using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Object = UnityEngine.Object;

namespace DefendTheWave.Common.Services.Spawn.Pooling.AddressablesPooling
{
    /// <summary>
    /// Took this pool https://github.com/Haruma-K/Addler/
    /// </summary>
    public sealed class AddressablePool : IDisposable
    {
        private readonly Dictionary<string, PooledObject> _busyObjects = new();
        private readonly Stack<GameObject> _usableObjects = new();
        private bool _isWarmingUp;

        public AddressablePool(object key, string poolName)
        {
            Key = key;
            Capacity = -1;
            Parent = new GameObject(poolName);
            Parent.transform.position = Vector2.one * 100;
            Object.DontDestroyOnLoad(Parent);
        }

        public GameObject Parent { get; }

        public object Key { get; }

        public bool IsDisposed { get; private set; }

        public int Capacity { get; private set; }

        public int UsableObjectsCount => _usableObjects.Count;

        public void Dispose()
        {
            if (IsDisposed)
                return;

            foreach (var handle in _busyObjects.Values)
                Addressables.ReleaseInstance(handle.Instance);

            foreach (var obj in _usableObjects)
                Addressables.ReleaseInstance(obj);

            Capacity = 0;
            _busyObjects.Clear();
            _usableObjects.Clear();

            if (Parent != null && !Parent.Equals(null))
                Object.Destroy(Parent);

            IsDisposed = true;
        }

        public async UniTask Preload(int capacity)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            if (_isWarmingUp)
                throw new InvalidOperationException(
                    $"This operation cannot be performed until the running {nameof(Preload)} is complete.");

            _isWarmingUp = true;
            Capacity = capacity;
            var diffCount = capacity - _busyObjects.Count - _usableObjects.Count;
            if (diffCount >= 1)
            {
                var instantiateHandles = new List<AsyncOperationHandle>();
                for (var i = 0; i < diffCount; i++)
                {
                    var instantiateHandle = Addressables.InstantiateAsync(Key, Parent.transform);
                    instantiateHandles.Add(instantiateHandle);
                }

                var instantiateGroupHandle =
                    Addressables.ResourceManager.CreateGenericGroupOperation(instantiateHandles);

                while (!instantiateGroupHandle.IsDone)
                {
                    await UniTask.Yield();
                }

                if (instantiateGroupHandle.Status == AsyncOperationStatus.Failed)
                    ExceptionDispatchInfo.Capture(instantiateGroupHandle.OperationException).Throw();

                foreach (var handle in instantiateGroupHandle.Result)
                {
                    var instance = handle.Convert<GameObject>().Result;
                    instance.SetActive(false);
                    _usableObjects.Push(instance);
                }
            }
            else if (diffCount <= -1)
            {
                for (var i = 0; i < -diffCount; i++)
                {
                    var obj = _usableObjects.Pop();
                    Addressables.ReleaseInstance(obj);
                }
            }

            _isWarmingUp = false;
        }

        public PooledObject Get()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (_usableObjects.Count == 0)
                throw new InvalidOperationException(
                    "There are no waiting objects available in ObjectPool. " +
                    $"You can expand the pool by calling {nameof(Preload)}.");

            if (_isWarmingUp)
                throw new InvalidOperationException(
                    $"This operation cannot be performed until the running {nameof(Preload)} is complete.");

            var instance = _usableObjects.Pop();

            // It seems that this instance has been destroyed outside the pool.
            if (instance == null)
                throw new InvalidOperationException(
                    "It seems that a GameObject you are trying to use has been destroyed outside the pool.");

            instance.SetActive(true);
            var handle = new PooledObject(this, instance);
            _busyObjects.Add(handle.Id, handle);
            return handle;
        }
        
        public void Return(PooledObject obj)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            var instance = obj.Instance;

            // If the returned instance has been destroyed outside the pool, do nothing.
            // InvalidOperationException will be thrown the next time this instance is retrieved from the pool.
            if (instance == null)
                return;

            instance.transform.SetParent(Parent.transform);
            instance.SetActive(false);
            instance.transform.DOKill();

            _busyObjects.Remove(obj.Id);
            _usableObjects.Push(obj.Instance);
        }
    }
}

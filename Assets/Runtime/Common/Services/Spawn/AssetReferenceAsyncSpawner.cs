using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DefendTheWave.Common.Services.Spawn
{
	public class AssetReferenceAsyncSpawner : IAsyncSpawner<AssetReferenceSpawnResource, AssetReferenceSpawnResourceProvider>
	{
		private Queue<AsyncOperationHandle<GameObject>> _handles = new();

		private bool _disposing;

		public async UniTask<ISpawnableEntity> SpawnAsync(AssetReferenceSpawnResourceProvider resourceProvider, CancellationToken token)
		{
			var handle = Addressables.InstantiateAsync(resourceProvider.GetSpawnResource().SpawnResource);
			
			_handles.Enqueue(handle);

			var result = (await handle.WithCancellation(cancellationToken: token)).GetComponent<ISpawnableEntity>();
			
			result.OnSpawned();
			
			return result;
		}

		void IDisposable.Dispose()
		{
			if (_disposing == true)
			{
				return;
			}
			
			_disposing = true;

			DisposeHandles();

			void DisposeHandles()
			{
				var disposeTasks = new Queue<UniTask>(_handles.Count);
				
				foreach (var operation in _handles)
				{
					disposeTasks.Enqueue(UniTask.WaitUntil(() => operation.IsDone).ContinueWith(() => Addressables.Release(operation)));
				}

				_handles.Clear();

				UniTask.WhenAll(disposeTasks).ContinueWith(() => _disposing = false).Forget();
			}
		}
	}
}

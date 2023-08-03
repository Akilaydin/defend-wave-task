using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DefendTheWave.Common.Services.Spawn
{
	public class AssetReferenceAsyncSpawner : IAsyncSpawner<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>>
	{
		private Queue<AsyncOperationHandle<GameObject>> _handles = new();

		private bool _disposing;
		
		private ISpawnResourceProvider<AssetReferenceSpawnResource> _resourceProvider;

		void IAsyncSpawner<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>>.SetResourceProvider(ISpawnResourceProvider<AssetReferenceSpawnResource> resourceProvider)
		{
			_resourceProvider = resourceProvider;
		}

		public async UniTask<ISpawnableEntity> SpawnAsync(CancellationToken token)
		{
			Assert.IsNotNull(_resourceProvider);
			
			var handle = Addressables.InstantiateAsync(_resourceProvider.GetSpawnResource().SpawnResource);
			
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
					disposeTasks.Enqueue(UniTask.WaitUntil(() => operation.IsDone).ContinueWith(() =>
					{
						if (operation.IsValid())
						{
							Addressables.Release(operation);
						}
					}));
				}

				_handles.Clear();

				UniTask.WhenAll(disposeTasks).ContinueWith(() => _disposing = false).Forget();
			}
		}
	}
}

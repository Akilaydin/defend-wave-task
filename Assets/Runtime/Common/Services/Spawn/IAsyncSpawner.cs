using System;
using System.Threading;

using Cysharp.Threading.Tasks;

namespace DefendTheWave.Common.Services.Spawn
{
	public interface IAsyncSpawner<TSpawnResource, in TSpawnResourceProvider> : IDisposable
		where TSpawnResource : ISpawnResource
		where TSpawnResourceProvider : ISpawnResourceProvider<TSpawnResource>
	{
		public void SetResourceProvider(TSpawnResourceProvider resourceProvider);
		public UniTask<ISpawnableEntity> SpawnAsync(CancellationToken token);
	}
}
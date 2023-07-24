using System;
using System.Threading;

using Cysharp.Threading.Tasks;

namespace DefendTheWave.Common.Services.Spawn
{
	public interface IAsyncSpawner<TSpawnResource, in TSpawnResourceProvider> : IDisposable
		where TSpawnResource : ISpawnResource
		where TSpawnResourceProvider : ISpawnResourceProvider<TSpawnResource>
	{
		public UniTask<ISpawnableEntity> SpawnAsync(TSpawnResourceProvider resourceProvider, CancellationToken token);
	}
}
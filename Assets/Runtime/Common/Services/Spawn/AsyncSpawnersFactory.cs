using VContainer;

namespace DefendTheWave.Common.Services.Spawn
{
	public class AsyncSpawnersFactory<TSpawnResource, TSpawnResourceProvider> : Disposable
		where TSpawnResource : ISpawnResource
		where TSpawnResourceProvider : ISpawnResourceProvider<TSpawnResource>
	{
		[Inject] private readonly IObjectResolver _resolver;
		
		public IAsyncSpawner<TSpawnResource, TSpawnResourceProvider> GetAsyncSpawner(TSpawnResourceProvider spawnResourceProvider)
		{
			var spawner = _resolver.Resolve<IAsyncSpawner<TSpawnResource, TSpawnResourceProvider>>();
			
			spawner.SetResourceProvider(spawnResourceProvider);
			
			CompositeDisposable.Add(spawner);

			return spawner;
		}
	}
}

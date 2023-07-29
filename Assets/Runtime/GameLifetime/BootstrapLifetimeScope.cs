using DefendTheWave.Common.Services.Spawn;
using DefendTheWave.Input;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class BootstrapLifetimeScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<KeyboardInputService>(Lifetime.Singleton).AsImplementedInterfaces();
			
			builder.Register<AssetReferenceAsyncSpawner>(Lifetime.Transient).AsImplementedInterfaces();
			builder.Register<AsyncSpawnersFactory<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>>>(Lifetime.Singleton).AsSelf();
		}
	}
}

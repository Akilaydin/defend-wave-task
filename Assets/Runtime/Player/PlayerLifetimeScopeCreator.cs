using System.Threading;

using Cysharp.Threading.Tasks;

using DefendTheWave.Common;
using DefendTheWave.Common.Services.Spawn;
using DefendTheWave.Common.Services.Spawn.Pooling;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Player.Health;
using DefendTheWave.Player.Movement;
using DefendTheWave.Player.Shooting;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Player
{
	public class PlayerLifetimeScopeCreator : Disposable, IAsyncStartable
	{
		[Inject] private readonly LevelSceneData _levelSceneData;
		[Inject] private readonly LifetimeScope _parentScope;

		[Inject] private readonly AsyncSpawnersFactory<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _spawnersFactory;
		[Inject] private readonly SpawnablePlayerSettings _spawnablePlayerSettings;

		async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
		{
			var playerSpawner = _spawnersFactory.GetAsyncSpawner(_spawnablePlayerSettings);

			var player = (PlayerView)await playerSpawner.SpawnAsync(cancellation);

			player.transform.SetParent(_levelSceneData.PlayerSpawnRoot, false);

			using (LifetimeScope.EnqueueParent(_parentScope))
			{
				LifetimeScope.Create(CreatePlayerScope);
			}
			
			void CreatePlayerScope(IContainerBuilder builder)
			{
				builder.RegisterInstance(_levelSceneData.PlayerHealthView);
				builder.RegisterInstance(_spawnablePlayerSettings.SpawnableBulletSettings);
				
				builder.Register<PlayerClampedPositionProvider>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

				builder.Register<PlayerMovementController>(Lifetime.Scoped).AsImplementedInterfaces();
				
				builder.Register<PlayerShootingController>(Lifetime.Scoped).AsImplementedInterfaces();
				builder.Register<BulletsPool>(Lifetime.Scoped).AsImplementedInterfaces();
				
				builder.Register<PlayerContainer>(Lifetime.Scoped).AsSelf().WithParameter(player);

				builder.Register<PlayerHealthModel>(Lifetime.Scoped).AsSelf();
				builder.Register<PlayerHealthController>(Lifetime.Scoped).AsSelf();
			}
		}
	}
}

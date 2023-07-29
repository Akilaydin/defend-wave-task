using System.Threading;

using Cysharp.Threading.Tasks;

using DefendTheWave.Common;
using DefendTheWave.Common.Services.Spawn;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Player.Health;
using DefendTheWave.Player.Movement;

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
			
			var player = (PlayerView) await playerSpawner.SpawnAsync(cancellation);
			
			player.transform.SetParent(_levelSceneData.PlayerSpawnRoot, false);
			
			CompositeDisposable.Add(_parentScope.CreateChild(CreatePlayerScope));

			void CreatePlayerScope(IContainerBuilder builder)
			{
				builder.Register<PlayerClampedPositionProvider>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
				
				builder.Register<PlayerMovementController>(Lifetime.Scoped).AsImplementedInterfaces();
				builder.Register<PlayerContainer>(Lifetime.Scoped).AsSelf().WithParameter(player);
				
				builder.RegisterInstance(_levelSceneData.PlayerHealthView);
				builder.Register<PlayerHealthModel>(Lifetime.Scoped).AsSelf();
				builder.Register<PlayerHealthController>(Lifetime.Scoped).AsSelf();
			}
		}
	}
}

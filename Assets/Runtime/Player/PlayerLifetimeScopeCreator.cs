using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DefendTheWave.Common.Services.Spawn;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Player.Movement;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Player
{
	public class PlayerLifetimeScopeCreator : IAsyncStartable, IDisposable
	{
		[Inject] private readonly SpawnablePlayerSettings _spawnablePlayerSettings;
		[Inject] private readonly LevelSceneData _levelSceneData;
		[Inject] private readonly LifetimeScope _parentScope;
		
		[Inject] private readonly ISpawnResourceProvider<AssetReferenceSpawnResource> _spawnResourceProvider;
		[Inject] private readonly AsyncSpawnersFactory<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _spawnersFactory;

		private LifetimeScope _playerScope;
		
		private IAsyncSpawner<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _spawner;

		async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
		{
			_spawner = _spawnersFactory.GetAsyncSpawner(_spawnResourceProvider);
			
			var player = await _spawner.SpawnAsync(cancellation) as PlayerView;
			
			player!.transform.SetParent(_levelSceneData.PlayerSpawnRoot, false);
			
			_playerScope = _parentScope.CreateChild(CreatePlayerScope);

			void CreatePlayerScope(IContainerBuilder builder)
			{
				builder.Register<PlayerMovementController>(Lifetime.Scoped).AsImplementedInterfaces();
				builder.Register<PlayerContainer>(Lifetime.Scoped).AsSelf().WithParameter(player);
			}
		}

		void IDisposable.Dispose()
		{
			_spawner.Dispose();
			
			_playerScope.Dispose();
		}
	}
}

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
		[Inject] private readonly AssetReferenceSpawnResourceProvider _spawnResourceProvider;
		[Inject] private readonly LifetimeScope _parentScope;
		
		[Inject] private readonly IAsyncSpawner<AssetReferenceSpawnResource, AssetReferenceSpawnResourceProvider> _spawner;

		private LifetimeScope _playerScope;

		async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
		{
			var player = await _spawner.SpawnAsync(_spawnResourceProvider, cancellation) as PlayerView;
			
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

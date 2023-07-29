using DefendTheWave.Common;
using DefendTheWave.Common.Services;
using DefendTheWave.Common.Services.Spawn;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Player;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class LevelLifetimeScope : LifetimeScope
	{
		[SerializeField] private LevelSceneData _levelSceneData;
		[SerializeField] private GlobalGameSettings _gameSettings;
		[SerializeField] private SpawnablePlayerSettings _spawnablePlayerSettings;
		[SerializeField] private SpawnableEnemySettings[] _enemiesSettings;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(_levelSceneData);
			builder.RegisterInstance(_gameSettings);
			builder.RegisterInstance(_spawnablePlayerSettings);
			builder.RegisterInstance(_enemiesSettings);
			
			builder.Register<ScreenBoundsProvider>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
			builder.Register<PlayerClampedPositionProvider>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
			
			builder.Register<AsyncSpawnersFactory<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>>>(Lifetime.Singleton).AsSelf();

			builder.Register<AssetReferenceAsyncSpawner>(Lifetime.Scoped).AsImplementedInterfaces();
			builder.Register<AssetReferenceSpawnResourceProvider>(Lifetime.Transient).AsImplementedInterfaces().WithParameter(_spawnablePlayerSettings.SpawnableEntityPrefab);

			builder.RegisterEntryPoint<PlayerLifetimeScopeCreator>();
		}
	}
}

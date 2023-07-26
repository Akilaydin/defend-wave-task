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
			
			builder.Register<ScreenBoundsProvider>(Lifetime.Scoped).AsSelf();
			builder.Register<ScreenClampedPositionProvider>(Lifetime.Scoped).AsSelf();
			
			builder.Register<AssetReferenceAsyncSpawner>(Lifetime.Scoped).AsImplementedInterfaces();
			builder.Register<AssetReferenceSpawnResource>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();
			builder.Register<AssetReferenceSpawnResourceProvider>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf().WithParameter(_spawnablePlayerSettings.SpawnableEntityPrefab);

			builder.RegisterEntryPoint<PlayerLifetimeScopeCreator>();
		}
	}
}

using DefendTheWave.Common.Services;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.GUI.Controllers;
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
			builder.RegisterInstance(_levelSceneData.LostPopup);
			builder.RegisterInstance(_levelSceneData.VictoryPopup);
			
			builder.Register<ScreenBoundsProvider>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
			
			builder.Register<PlayerLifetimeScopeCreator>(Lifetime.Scoped).AsImplementedInterfaces();
			builder.Register<LostPopupController>(Lifetime.Scoped).AsImplementedInterfaces();
			builder.Register<VictoryPopupController>(Lifetime.Scoped).AsImplementedInterfaces();
			
			builder.Register<GameStateModel>(Lifetime.Scoped).AsSelf();
			builder.Register<GameRestarter>(Lifetime.Scoped).AsSelf();
		}
	}
}

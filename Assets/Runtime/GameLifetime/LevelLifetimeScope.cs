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
			
			//note: All entry points to fix the known framework issue https://github.com/hadashiA/VContainer/issues/319
			builder.RegisterEntryPoint<PlayerLifetimeScopeCreator>();
			
			builder.RegisterEntryPoint<LostPopupController>();
			builder.RegisterEntryPoint<VictoryPopupController>();
			
			builder.RegisterEntryPoint<ScreenBoundsProvider>(Lifetime.Scoped).AsSelf();

			builder.RegisterEntryPoint<GameStateModel>().AsSelf();
			builder.RegisterEntryPoint<GameRestarter>().AsSelf();
		}
	}
}

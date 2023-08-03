using DefendTheWave.Common.Services;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Enemies;
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
		[SerializeField] private EnemiesPack _enemies;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(_levelSceneData);
			builder.RegisterInstance(_gameSettings);
			builder.RegisterInstance(_spawnablePlayerSettings);
			builder.RegisterInstance(_enemies);
			builder.RegisterInstance(_levelSceneData.LostPopup);
			builder.RegisterInstance(_levelSceneData.VictoryPopup);
			
			//note: All entry points to fix the known framework issue https://github.com/hadashiA/VContainer/issues/319
			builder.RegisterEntryPoint<PlayerLifetimeScopeCreator>();
			
			builder.RegisterEntryPoint<EnemiesSpawningController>();
			
			builder.RegisterEntryPoint<LostPopupController>();
			builder.RegisterEntryPoint<VictoryPopupController>();

			builder.RegisterEntryPoint<ScreenBoundsProvider>().AsSelf();

			builder.RegisterEntryPoint<GameStateController>();
			builder.RegisterEntryPoint<GameStateModel>();
			builder.RegisterEntryPoint<GameEventBus>();
			builder.RegisterEntryPoint<GameRestarter>().AsSelf();
		}
	}
}

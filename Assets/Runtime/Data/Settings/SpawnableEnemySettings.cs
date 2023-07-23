using System.Linq;

using DefendTheWave.Common;

using EasyButtons;

using UnityEditor;

using UnityEngine;

namespace DefendTheWave.Data.Settings
{
	[CreateAssetMenu(fileName = nameof(SpawnableEnemySettings), menuName = GameConstants.ScriptableObjectsRoot + nameof(SpawnableEnemySettings), order = 0)]
	public class SpawnableEnemySettings : BaseSpawnableEntitySettings
	{
		[field: SerializeField] public EnemySettings EnemySettings { get; private set; }

		#if UNITY_EDITOR
		private void Reset()
		{
			SetGlobalSettingsAsCurrent();
		}
		
		[Button]
		private void SetGlobalSettingsAsCurrent()
		{
			const string configsPath = "Assets/Content/Configs";
			
			string[] assetNames = AssetDatabase.FindAssets(nameof(GlobalGameSettings), new[] { configsPath });

			if (assetNames.Length == 0)
			{
				Debug.LogError($"No {nameof(GlobalGameSettings)} was found at path {configsPath}");
			}
			
			var settingsPath = AssetDatabase.GUIDToAssetPath(assetNames.First());
			
			var globalGameSettings = AssetDatabase.LoadAssetAtPath<GlobalGameSettings>(settingsPath);

			EnemySettings = (EnemySettings) globalGameSettings.DefaultEnemiesSettings.Clone();
		}
		#endif
	}
}
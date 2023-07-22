using DefendTheWave.Common;

using UnityEngine;

namespace DefendTheWave.Settings
{
	[CreateAssetMenu(fileName = nameof(EnemySettings), menuName = GameConstants.ScriptableObjectsRoot + nameof(EnemySettings), order = 0)]
	public class EnemySettings : BaseSpawnableEntitySettings
	{
		[Space]
		[SerializeField] private bool _overrideGlobalGameSettings;
		
		private void OnValidate()
		{
			if (_overrideGlobalGameSettings == true)
			{
				return;
			}
			
			SetGlobalSettingsAsCurrent();

			void SetGlobalSettingsAsCurrent()
			{
				
			}
		}
	}
}

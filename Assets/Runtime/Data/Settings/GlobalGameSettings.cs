using DefendTheWave.Common;

using UnityEngine;

namespace DefendTheWave.Data.Settings
{
	[CreateAssetMenu(fileName = nameof(GlobalGameSettings), menuName = GameConstants.ScriptableObjectsRoot + nameof(GlobalGameSettings), order = 0)]
	public class GlobalGameSettings : ScriptableObject
	{
		[field: SerializeField, MinMaxSlider(10, 20)] public Vector2 DefeatedEnemiesCountToWin { get; private set; }
		[field: SerializeField, Range(1, 99)] public int LeakedEnemiesCountToLose { get; private set; }
		[field: SerializeField] public LayerMask EnemiesLayer { get; private set; }
		[field: SerializeField, MinMaxSlider(0.65f, 2f)] public Vector2 EnemiesSpawnCooldown { get; private set; }
		[field: SerializeField] public EnemySettings DefaultEnemiesSettings { get; private set; }
	}
}

using DefendTheWave.Common;

using UnityEngine;

namespace DefendTheWave.Data.Settings
{
	[CreateAssetMenu(fileName = nameof(SpawnablePlayerSettings), menuName = GameConstants.ScriptableObjectsRoot + nameof(SpawnablePlayerSettings), order = 0)]
	public class SpawnablePlayerSettings : BaseSpawnableEntitySettings
	{
		[field: SerializeField] public SpawnableBulletSettings SpawnableBulletSettings { get; private set; }
		
		[field: SerializeField, Range(1, 3f)] public int Damage { get; private set; }	
		[field: SerializeField, Min(0f)] public float BlastRadius { get; private set; }
		[field: SerializeField, Min(0f)] public float RateOfFire { get; private set; }
		[field: SerializeField, Min(0f)] public float ProjectileSpeed { get; private set; }
		[field: SerializeField, Min(0f)] public float PlayerSpeed { get; private set; }
	}
}

using DefendTheWave.Common;

using UnityEngine;

namespace DefendTheWave.Settings
{
	[CreateAssetMenu(fileName = nameof(SpawnablePlayerSettings), menuName = GameConstants.ScriptableObjectsRoot + nameof(SpawnablePlayerSettings), order = 0)]
	public class SpawnablePlayerSettings : BaseSpawnableEntitySettings
	{
		[field: SerializeField, Range(1f, 3f)] public float BlastRadius { get; private set; }
		[field: SerializeField, Range(0.25f, 5f)] public float RateOfFire { get; private set; }
		[field: SerializeField, Range(1, 3f)] public int Damage { get; private set; }
		[field: SerializeField, Range(1f, 5f)] public float ProjectileSpeed { get; private set; }
		[field: SerializeField, Range(1f, 5f)] public float PlayerSpeed { get; private set; }
	}
}

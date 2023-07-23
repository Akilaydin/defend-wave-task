using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefendTheWave.Data.Settings
{
	public abstract class BaseSpawnableEntitySettings : ScriptableObject
	{
		[field: SerializeField] public AssetReferenceGameObject SpawnableEntityPrefab { get; private set; }
	}
}

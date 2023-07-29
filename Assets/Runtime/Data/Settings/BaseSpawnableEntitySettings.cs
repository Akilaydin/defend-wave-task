using DefendTheWave.Common.Services.Spawn;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefendTheWave.Data.Settings
{
	public abstract class BaseSpawnableEntitySettings : ScriptableObject, ISpawnResourceProvider<AssetReferenceSpawnResource>
	{
		[SerializeField] private AssetReference _spawnableEntityPrefab;

		public AssetReferenceSpawnResource GetSpawnResource()
		{
			return new AssetReferenceSpawnResource(_spawnableEntityPrefab);
		}
	}
}

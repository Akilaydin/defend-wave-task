using UnityEngine.AddressableAssets;

namespace DefendTheWave.Common.Services.Spawn
{
	public class AssetReferenceSpawnResource : ISpawnResource
	{
		public AssetReference SpawnResource { get; }
		
		public AssetReferenceSpawnResource(AssetReference spawnResource)
		{
			SpawnResource = spawnResource;
		}
	}
}

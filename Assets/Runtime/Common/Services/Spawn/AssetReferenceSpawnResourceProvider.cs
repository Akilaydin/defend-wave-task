using UnityEngine.AddressableAssets;

namespace DefendTheWave.Common.Services.Spawn
{
	public class AssetReferenceSpawnResourceProvider : ISpawnResourceProvider<AssetReferenceSpawnResource>
	{
		private readonly AssetReferenceSpawnResource _spawnResource;
		
		public AssetReferenceSpawnResourceProvider(AssetReference reference)
		{
			_spawnResource = new AssetReferenceSpawnResource(reference);
		}
		
		public AssetReferenceSpawnResource GetSpawnResource()
		{
			return _spawnResource;
		}
	}
}

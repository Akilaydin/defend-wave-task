using Cysharp.Threading.Tasks;

using DefendTheWave.Player;
using DefendTheWave.Player.Shooting;

using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Common.Services.Spawn.Pooling
{
	public class BulletsPool : IAsyncObjectPool<BulletView>, IInitializable
	{
		[Inject] private readonly ISpawnResourceProvider<AssetReferenceSpawnResource> _spawnResourceProvider;

		private ObjectPool<BulletView> _innerPool;

		private AssetReference _bulletReference;
		
		void IInitializable.Initialize()
		{
			_bulletReference = _spawnResourceProvider.GetSpawnResource().SpawnResource;
		}

		private void OnGotBulletFromPool(BulletView bullet)
		{
			bullet.OnGotFromPool();
		}
		
		private void OnBulletReleased(BulletView bullet)
		{
			bullet.OnReturnedToPool();
		}
		
		private void OnBulletDestroyed(BulletView bullet)
		{
			bullet.OnDestroyed();
		}

		public async UniTask<BulletView> GetAsync()
		{
			var spawnedBullet = (await _bulletReference.InstantiateAsync()).GetComponent<BulletView>();
			
			spawnedBullet.OnCreated();
			
			return spawnedBullet;
		}

		public void ReturnToPool(BulletView poolableObject)
		{
			poolableObject.OnReturnedToPool();
		}
	}
}

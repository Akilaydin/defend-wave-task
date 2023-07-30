using System.Threading;

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
		[Inject] private readonly ISpawnResourceProvider<AssetReferenceSpawnResource> _bulletSpawnResource;
		[Inject] private readonly AsyncSpawnersFactory<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _spawnersFactory;

		private ObjectPool<BulletView> _innerPool;
		private IAsyncSpawner<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _bulletsSpawner;

		void IInitializable.Initialize()
		{
			_bulletsSpawner = _spawnersFactory.GetAsyncSpawner(_bulletSpawnResource);
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

		public async UniTask<BulletView> GetAsync(CancellationToken token)
		{
			var spawnedBullet = (await _bulletsSpawner.SpawnAsync(token));
			
			return (BulletView) spawnedBullet;
		}

		public void ReturnToPool(BulletView poolableObject)
		{
			poolableObject.OnReturnedToPool();
		}
	}
}

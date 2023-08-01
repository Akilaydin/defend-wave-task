using System.Threading;

using Cysharp.Threading.Tasks;

using DefendTheWave.Data.Settings;
using DefendTheWave.Player.Shooting;

using UnityEngine.Pool;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Common.Services.Spawn.Pooling
{
	public class BulletsPool : IAsyncObjectPool<BulletView>, IInitializable
	{
		[Inject] private readonly SpawnableBulletSettings _spawnableBulletSettings;
		[Inject] private readonly AsyncSpawnersFactory<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _spawnersFactory;

		private IAsyncSpawner<AssetReferenceSpawnResource, ISpawnResourceProvider<AssetReferenceSpawnResource>> _bulletsSpawner;

		void IInitializable.Initialize()
		{
			_bulletsSpawner = _spawnersFactory.GetAsyncSpawner(_spawnableBulletSettings);
		}

		private void OnGotBulletFromPool(IPoolableObject bullet)
		{
			bullet.OnGotFromPool();
		}
		
		private void OnBulletReleased(IPoolableObject bullet)
		{
			bullet.OnReturnedToPool();
		}
		
		private void OnBulletDestroyed(IPoolableObject bullet)
		{
			bullet.OnDestroyed();
		}

		public async UniTask<BulletView> GetAsync(CancellationToken token)
		{
			var spawnedBullet = (await _bulletsSpawner.SpawnAsync(token)) as IPoolableObject;
			
			OnGotBulletFromPool(spawnedBullet);
			
			return (BulletView) spawnedBullet;
		}

		public void ReturnToPool(BulletView poolableObject)
		{
			poolableObject.OnReturnedToPool();
		}
	}
}

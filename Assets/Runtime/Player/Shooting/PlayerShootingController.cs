using System;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common.Services;
using DefendTheWave.Common.Services.Spawn.Pooling;
using DefendTheWave.Data.Settings;

using DG.Tweening;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.Player.Shooting
{
	public class PlayerShootingController : Disposable, IInitializable
	{
		private const float BulletReleaseOffset = 3f;
			
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;
		[Inject] private readonly IAsyncObjectPool<BulletView> _bulletsPool;
		[Inject] private readonly ScreenBoundsProvider _screenBoundsProvider;

		private Bounds _screenBounds;
		private float _projectileSpeed;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_playerSettings.RateOfFire)).Subscribe(ShootAsync));

			_screenBounds = _screenBoundsProvider.GetScreenBounds();
			_projectileSpeed = _playerSettings.ProjectileSpeed;
		}

		private async UniTaskVoid ShootAsync(AsyncUnit _)
		{
			var bullet = await _bulletsPool.GetAsync();

			bullet.transform.DOMoveY(_screenBounds.max.y + BulletReleaseOffset, _projectileSpeed).SetSpeedBased(true).OnComplete(() =>
			{
				ReturnBulletToPool(bullet);		
			});
		}

		private void ReturnBulletToPool(BulletView bulletView)
		{
			_bulletsPool.ReturnToPool(bulletView);
		}
	}
}

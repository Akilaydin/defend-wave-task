using System;
using System.Threading;

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
			CompositeDisposable.Add(UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_playerSettings.RateOfFire)).SubscribeAwait(ShootAsync));

			_screenBounds = _screenBoundsProvider.GetScreenBounds();
			_projectileSpeed = _playerSettings.ProjectileSpeed;
		}
		
#pragma warning disable CS4014
		private async UniTask ShootAsync(AsyncUnit _, CancellationToken token)
		{
			var bullet = await _bulletsPool.GetAsync(token);

			bullet.transform.DOMoveY(_screenBounds.max.y + BulletReleaseOffset, _projectileSpeed).SetSpeedBased(true).OnComplete(() =>
			{
				ReturnBulletToPool(bullet);		
			});
		}
#pragma warning restore CS4014

		private void ReturnBulletToPool(BulletView bulletView)
		{
			_bulletsPool.ReturnToPool(bulletView);
		}
	}
}

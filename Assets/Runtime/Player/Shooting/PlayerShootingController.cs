using System;
using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common.Services;
using DefendTheWave.Common.Services.Spawn.Pooling;
using DefendTheWave.Data.Settings;
using DefendTheWave.Enemies;

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
			
		[Inject] private readonly GlobalGameSettings _globalGameSettings;
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;
		[Inject] private readonly IAsyncObjectPool<BulletView> _bulletsPool;
		[Inject] private readonly ScreenBoundsProvider _screenBoundsProvider;
		[Inject] private readonly PlayerContainer _playerContainer;

		private PlayerView _player;
		
		private Bounds _screenBounds;
		private float _projectileSpeed;
		private int _enemiesLayer;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_playerSettings.RateOfFire)).SubscribeAwait(ShootAsync));
			CompositeDisposable.Add(_playerContainer.Player.Subscribe(CachePlayer));
			
			_screenBounds = _screenBoundsProvider.GetScreenBounds();
			_projectileSpeed = _playerSettings.ProjectileSpeed;

			_enemiesLayer = _globalGameSettings.EnemiesLayer;
		}

		private void CachePlayer(PlayerView playerView)
		{
			_player = playerView;
		}
		
#pragma warning disable CS4014
		private async UniTask ShootAsync(AsyncUnit _, CancellationToken token)
		{
			if (EnemyInBlastRange(out var closestEnemyPosition))
			{
				var bullet = await _bulletsPool.GetAsync(token);

				bullet.transform.DOMove(closestEnemyPosition + Vector2.one * BulletReleaseOffset, _projectileSpeed).SetSpeedBased(true).OnComplete(() =>
				{
					ReturnBulletToPool(bullet);		
				});
			}
		}
#pragma warning restore CS4014

		private bool EnemyInBlastRange(out Vector2 closestEnemyPosition)
		{
			closestEnemyPosition = Vector2.zero;

			var results = new RaycastHit2D[8];

			var hitCount = Physics2D.BoxCastNonAlloc(_player.transform.position, _screenBounds.size, 0, Vector2.up, results, _playerSettings.BlastRadius, _enemiesLayer);

			if (hitCount == 0)
			{
				return false;
			}
			
			if (hitCount == 1)
			{
				closestEnemyPosition = results[0].transform.position;
				
				return true;
			}
			
			closestEnemyPosition = results.Select(result => GetClosest(result.transform.position, _player.transform.position)).Min();
			
			return true;

			Vector2 GetClosest(Vector2 v1, Vector2 v2)
			{
				return Vector2.Distance(v1, v2) >= Vector2.Distance(v2, v1) ? v1 : v2;
			}
		}

		private void ReturnBulletToPool(BulletView bulletView)
		{
			_bulletsPool.ReturnToPool(bulletView);
		}
	}
}

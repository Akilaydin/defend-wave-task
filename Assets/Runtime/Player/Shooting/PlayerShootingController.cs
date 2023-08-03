using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common.Services;
using DefendTheWave.Common.Services.Spawn.Pooling;
using DefendTheWave.Common.Services.Spawn.Pooling.AddressablesPooling;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.GameLifetime;
using DefendTheWave.GameLifetime.Interfaces;

using DG.Tweening;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.Player.Shooting
{
	public class PlayerShootingController : Disposable, IStartable, IAsyncStartable
	{
		private const float BulletReleaseOffsetMultiplier = 1.5f;
		private const float RaycastBoxHeight = 0.5f;
		private const int PreloadedBulletsCount = 10;

		[Inject] private readonly GlobalGameSettings _globalGameSettings;
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;
		[Inject] private readonly ScreenBoundsProvider _screenBoundsProvider;
		[Inject] private readonly PlayerContainer _playerContainer;
		[Inject] private readonly LevelSceneData _sceneData;
		[Inject] private readonly SpawnableBulletSettings _spawnableBulletSettings;
		[Inject] private readonly IGameStateProvider _gameStateProvider;
		[Inject] private readonly IGameEventsInvoker _gameEventsInvoker;

		private List<Tween> _bulletMovementTweens = new(PreloadedBulletsCount);
		private PlayerView _player;
		private AddressablePool _addressablePool;

		private Bounds _screenBounds;
		private float _projectileSpeed;
		private float _blastRadius;
		private int _enemiesLayer;

		void IStartable.Start()
		{
			CompositeDisposable.Add(UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_playerSettings.RateOfFire)).Subscribe(Shoot));
			CompositeDisposable.Add(_playerContainer.Player.Subscribe(CachePlayer));
			
			_screenBounds = _screenBoundsProvider.GetScreenBounds();

			_projectileSpeed = _playerSettings.ProjectileSpeed;
			_blastRadius = _playerSettings.BlastRadius;

			_enemiesLayer = _globalGameSettings.EnemiesLayer;
			_addressablePool = new AddressablePool(_spawnableBulletSettings.GetSpawnResource().SpawnResource.RuntimeKey, _spawnableBulletSettings.GetSpawnResource().SpawnResource + " pool");
		}

		async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
		{
			await _addressablePool.Preload(PreloadedBulletsCount);
		}

		protected override void OnDispose()
		{
			foreach (var bulletMovementTween in _bulletMovementTweens)
			{
				if (bulletMovementTween.IsActive())
				{
					bulletMovementTween.Kill();
				}
			}
		}

		private void CachePlayer(PlayerView playerView)
		{
			_player = playerView;
		}
		
#pragma warning disable CS4014

		private void Shoot(AsyncUnit _)
		{
			if (EnemyInBlastRange(out var closestEnemyPosition))
			{
				var spawnedBullet = _addressablePool.Get();
                
				
				var bulletTransform = spawnedBullet.Instance.transform;
				
				bulletTransform.SetParent(_sceneData.BulletsSpawnRoot); 

				bulletTransform.position = _player.BulletsSpawnPoint.position;

				var direction = closestEnemyPosition - (Vector2) bulletTransform.position;

				var tween = bulletTransform.transform
					.DOMove(direction.normalized * _blastRadius * BulletReleaseOffsetMultiplier, _projectileSpeed)
					.SetSpeedBased(true);
				
				_bulletMovementTweens.Add(tween);

				spawnedBullet.Instance.GetComponent<BulletView>().HitEnemy += DisposeBullet;

				tween.OnComplete(DisposeBullet);

				void DisposeBullet()
				{
					_bulletMovementTweens.Remove(tween);
					
					spawnedBullet.Dispose();
				}
			}
		}
#pragma warning restore CS4014

		private bool EnemyInBlastRange(out Vector2 closestEnemyPosition)
		{
			closestEnemyPosition = Vector2.zero;

			var results = new RaycastHit2D[8];

			var hitCount = Physics2D.BoxCastNonAlloc(_player.transform.position, new Vector2(_screenBounds.size.x, RaycastBoxHeight), 0, Vector2.up, results, _playerSettings.BlastRadius, _enemiesLayer);
			
			if (hitCount == 0)
			{
				return false;
			}
			
			if (hitCount == 1)
			{
				closestEnemyPosition = results[0].transform.position;
				
				return true;
			}

			float minDistance = Mathf.Infinity;
			
			foreach (var result in results)
			{
				if (result.transform == null)
				{
					continue;
				}

				var currentDistance = Vector2.Distance(_player.transform.position, result.transform.position);
				
				if (minDistance < Vector2.Distance(_player.transform.position, result.transform.position))
				{
					continue;
				}

				minDistance = currentDistance;

				closestEnemyPosition = result.transform.position;
			}

			return true;
		}
	}
}

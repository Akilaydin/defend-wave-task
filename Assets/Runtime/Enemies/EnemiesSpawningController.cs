using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common;
using DefendTheWave.Common.Services.Spawn.Pooling.AddressablesPooling;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.GameLifetime.Interfaces;
using DefendTheWave.Player.Shooting;

using DG.Tweening;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Enemies
{
	public class EnemiesSpawningController : Disposable, IAsyncStartable, IGameEndedHandler
	{
		[Inject] private readonly LevelSceneData _levelSceneData;
		[Inject] private readonly GlobalGameSettings _gameSettings;
		[Inject] private readonly EnemiesPack _enemiesPack;
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;
		[Inject] private readonly IGameStateProvider _gameStateProvider;
		[Inject] private readonly IGameEventsInvoker _gameEventsInvoker;
		
		private Dictionary<SpawnableEnemySettings, AddressablePool> _enemyFactories;

		private List<EnemyModel> _spawnedEnemies = new();
		private IDisposable _enemySpawningSubscription;

		async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
		{
			_enemyFactories = new Dictionary<SpawnableEnemySettings, AddressablePool>(_enemiesPack.Enemies.Length);

			await PreloadEnemyPools();
			
			_enemySpawningSubscription = UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_gameSettings.EnemiesSpawnCooldown.GetRandomFromVector())).Subscribe(SpawnRandomEnemy);
            
			async UniTask PreloadEnemyPools()
			{
				foreach (var enemyInLevel in _enemiesPack.Enemies)
				{
					var pool = new AddressablePool(enemyInLevel.GetSpawnResource().SpawnResource.RuntimeKey,
						enemyInLevel.GetSpawnResource().SpawnResource + " pool");

					await pool.Preload(5);
				
					_enemyFactories.Add(enemyInLevel, pool);
				}
			}
		}
		
		void IGameEndedHandler.HandleGameEnded()
		{
			_enemySpawningSubscription.Dispose();
            
			var cachedSpawnedEnemies = _spawnedEnemies.ToArray();
			
			foreach (var spawnedEnemy in cachedSpawnedEnemies)
			{
				spawnedEnemy.Dispose();
			}
		}
		
		private void SpawnRandomEnemy(AsyncUnit _)
		{
			_enemyFactories.GetRandomElement().Deconstruct(out var settings, out var pool);

			var spawnedEnemy = pool.Get();

			var enemyModel = new EnemyModel(spawnedEnemy, settings.EnemySettings);

			_spawnedEnemies.Add(enemyModel);
			
			SubscribeToEnemy(enemyModel);
            
			MoveEnemy(enemyModel);
		}
		
		private void SubscribeToEnemy(EnemyModel enemyModel)
		{
			enemyModel.OnTriggerEnter += OnTriggerEnter;
			enemyModel.Destroyed += OnEnemyDestroyed;
			enemyModel.Disposed += () => _spawnedEnemies.Remove(enemyModel);

			void OnTriggerEnter(Collider2D collider2D)
			{
				if (_gameSettings.BulletsLayer.Includes(collider2D.gameObject.layer) == false)
				{
					return;
				}

				collider2D.GetComponent<BulletView>().InvokeHitEnemy();
                
				enemyModel.TakeDamage(_playerSettings.Damage);
			}

			void OnEnemyDestroyed()
			{
				enemyModel.Dispose();
				
				_gameEventsInvoker.InvokeEnemyDestroyed();
			}
		}

		private void MoveEnemy(EnemyModel enemyModel)
		{
			var randomSpawnPoint = _levelSceneData.EnemiesSpawnPoints.GetRandomElement();
            
			enemyModel.EnemyTransform.SetParent(randomSpawnPoint, false);
			enemyModel.EnemyTransform.localPosition = Vector2.zero;
            
			enemyModel.EnemyTransform.DOMoveY(_levelSceneData.FinishLineView.transform.position.y,enemyModel.EnemySettings.Speed.GetRandomFromVector())
				.OnComplete(() =>
				{
					_gameEventsInvoker.InvokeEnemyLeaked(enemyModel.EnemySettings.DamageOnLeak);
				
					enemyModel.Dispose();
				});
		}
	}
}

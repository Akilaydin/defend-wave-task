using System;

using DefendTheWave.Common.Services.Spawn.Pooling.AddressablesPooling;
using DefendTheWave.Data.Settings;

using UnityEngine;

namespace DefendTheWave.Enemies
{
	public class EnemyModel : IDisposable
	{
		public event Action Destroyed;
		public event Action Disposed;
		public event Action<Collider2D> OnTriggerEnter;

		public EnemySettings EnemySettings { get; private set; }
		public Transform EnemyTransform { get; private set; }
		
		private PooledObject PooledEnemy { get; }
		private int CurrentHealth { get; set; }
        
		public EnemyModel(PooledObject pooledEnemy, EnemySettings enemySettings)
		{
			PooledEnemy = pooledEnemy;
			EnemyTransform = pooledEnemy.Instance.transform;
			EnemySettings = enemySettings;

			pooledEnemy.Instance.GetComponent<BaseEnemyView>().OnTriggerEnterEvent += InvokeTriggerEnter;

			CurrentHealth = enemySettings.Health;
		}

		public void TakeDamage(int amount)
		{
			CurrentHealth -= amount;

			if (CurrentHealth <= 0)
			{
				Destroyed?.Invoke();
			}
		}
		
		private void InvokeTriggerEnter(Collider2D collider2D)
		{
			OnTriggerEnter?.Invoke(collider2D);
		}

		public void Dispose()
		{
			Disposed?.Invoke();
			
			Destroyed = null;
			OnTriggerEnter = null;
			Disposed = null;
			
			PooledEnemy.Dispose();
		}
	}
}

using System;

using DefendTheWave.GameLifetime.Interfaces;

namespace DefendTheWave.GameLifetime
{
	public class GameEventBus : IGameEventsProvider, IGameEventsInvoker
	{
		public event Action<int> EnemyLeaked;
		public event Action EnemyDestroyed;

		void IGameEventsInvoker.InvokeEnemyLeaked(int enemyDamageOnLeak)
		{
			EnemyLeaked?.Invoke(enemyDamageOnLeak);
		}

		void IGameEventsInvoker.InvokeEnemyDestroyed()
		{
			EnemyDestroyed?.Invoke();
		}
	}
}

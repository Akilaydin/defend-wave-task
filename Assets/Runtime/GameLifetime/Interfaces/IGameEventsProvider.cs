using System;

namespace DefendTheWave.GameLifetime.Interfaces
{
	public interface IGameEventsProvider
	{
		public event Action<int> EnemyLeaked;
		public event Action EnemyDestroyed;
	}
}

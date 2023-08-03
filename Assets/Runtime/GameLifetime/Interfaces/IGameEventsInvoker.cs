namespace DefendTheWave.GameLifetime.Interfaces
{
	public interface IGameEventsInvoker
	{
		public void InvokeEnemyLeaked(int damageOnLeak);

		public void InvokeEnemyDestroyed();
	}
}

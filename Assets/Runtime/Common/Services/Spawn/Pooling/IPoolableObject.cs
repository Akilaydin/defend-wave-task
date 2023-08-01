namespace DefendTheWave.Common.Services.Spawn.Pooling
{
	public interface IPoolableObject : ISpawnableEntity
	{
		void OnCreated();

		void OnGotFromPool();
		
		void OnReturnedToPool();
		
		void OnDestroyed();
	}
}

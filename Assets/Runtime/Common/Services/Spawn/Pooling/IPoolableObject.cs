namespace DefendTheWave.Common.Services.Spawn.Pooling
{
	public interface IPoolableObject
	{
		void OnCreated();

		void OnGotFromPool();
		
		void OnReturnedToPool();
		
		void OnDestroyed();
	}
}

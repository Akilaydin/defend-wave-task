namespace DefendTheWave.Common.Services.Spawn
{
	public interface ISpawnResourceProvider<out T> where T : ISpawnResource
	{
		public T GetSpawnResource();
	}
}

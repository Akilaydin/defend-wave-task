using DefendTheWave.GameLifetime;

namespace DefendTheWave.GameLifetime.Interfaces
{
	public interface IGameStateChanger
	{
		public void ChangeGameState(GameState newState);
	}
}

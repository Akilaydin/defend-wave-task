using Cysharp.Threading.Tasks;

namespace DefendTheWave.GameLifetime
{
	public class GameStateModel
	{
		public IReadOnlyAsyncReactiveProperty<GameState> CurrentGameState => _currentGameState;

		private AsyncReactiveProperty<GameState> _currentGameState = new(GameState.Entry);

		public void ChangeGameState(GameState newState)
		{
			_currentGameState.Value = newState;
		}
	}
}

using Cysharp.Threading.Tasks;

using DefendTheWave.GameLifetime.Interfaces;

namespace DefendTheWave.GameLifetime
{
	public class GameStateModel : IGameStateProvider, IGameStateChanger
	{
		IReadOnlyAsyncReactiveProperty<GameState> IGameStateProvider.CurrentGameState => _currentGameState;

		private AsyncReactiveProperty<GameState> _currentGameState = new(GameState.Entry);

		void IGameStateChanger.ChangeGameState(GameState newState)
		{
			_currentGameState.Value = newState;
		}
	}

}

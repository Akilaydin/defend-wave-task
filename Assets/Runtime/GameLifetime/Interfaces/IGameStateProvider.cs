using Cysharp.Threading.Tasks;

namespace DefendTheWave.GameLifetime.Interfaces
{
	public interface IGameStateProvider
	{
		public IReadOnlyAsyncReactiveProperty<GameState> CurrentGameState { get; }
	}
}

using Cysharp.Threading.Tasks;

namespace DefendTheWave.Player
{
	public class PlayerContainer
	{
		public IReadOnlyAsyncReactiveProperty<PlayerView> Player => _player;

		private AsyncReactiveProperty<PlayerView> _player;

		public PlayerContainer(PlayerView playerView)
		{
			_player = new AsyncReactiveProperty<PlayerView>(playerView);
		}
	}
}

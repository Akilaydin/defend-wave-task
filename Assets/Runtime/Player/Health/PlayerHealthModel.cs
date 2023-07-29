using Cysharp.Threading.Tasks;

using DefendTheWave.Data.Settings;

namespace DefendTheWave.Player.Health
{
	public class PlayerHealthModel
	{
		public IReadOnlyAsyncReactiveProperty<int> PlayerHealth => _playerHealth;

		private AsyncReactiveProperty<int> _playerHealth;

		public PlayerHealthModel(GlobalGameSettings gameSettings)
		{
			_playerHealth = new AsyncReactiveProperty<int>(gameSettings.LeakedEnemiesCountToLose);
		}
	}
}

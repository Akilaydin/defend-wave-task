using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Player.Health
{
	public class PlayerHealthController : Disposable, IInitializable
	{
		private const string HealthPrefix = "Здоровье: {0}";
		
		[Inject] private readonly PlayerHealthModel _playerHealthModel;
		[Inject] private PlayerHealthView _playerHealthView;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_playerHealthModel.PlayerHealth.Subscribe(ChangeHealthView));
		}

		private void ChangeHealthView(int currentHealth)
		{
			_playerHealthView.HealthText.text = string.Format(HealthPrefix, currentHealth);
		}
	}
}

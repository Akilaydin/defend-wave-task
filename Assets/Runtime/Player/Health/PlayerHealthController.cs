using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common;
using DefendTheWave.GameLifetime.Interfaces;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Player.Health
{
	public class PlayerHealthController : Disposable, IInitializable, IStartable
	{
		private const string HealthPrefix = "Здоровье: {0}";
		
		[Inject] private readonly PlayerHealthModel _playerHealthModel;
		[Inject] private PlayerHealthView _playerHealthView;
		[Inject] private readonly IGameEventsProvider _gameEventsProvider;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_playerHealthModel.PlayerHealth.Subscribe(ChangeHealthView));
		}
		
		void IStartable.Start()
		{
			_gameEventsProvider.EnemyLeaked += _playerHealthModel.DecreaseHealth;
		}

		protected override void OnDispose()
		{
			_gameEventsProvider.EnemyLeaked -= _playerHealthModel.DecreaseHealth;
		}

		private void ChangeHealthView(int currentHealth)
		{
			_playerHealthView.HealthText.text = string.Format(HealthPrefix, currentHealth);
		}
	}
}

using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Data.Settings;
using DefendTheWave.Input;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.Player.Movement
{
	public class PlayerMovementController : Disposable, IInitializable
	{
		[Inject] private readonly IInputService _inputService;
		[Inject] private readonly PlayerContainer _playerContainer;
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;

		private Transform _playerTransform;
		
		private float _playerSpeed;
		
		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_inputService.MovementVector.WithoutCurrent().Subscribe(MovePlayer));
			CompositeDisposable.Add(_playerContainer.Player.Subscribe(SetPlayerTransform));

			_playerSpeed = _playerSettings.PlayerSpeed;
		}

		private void SetPlayerTransform(PlayerView playerView)
		{
			_playerTransform = playerView.transform;
		}

		private void MovePlayer(Vector2 movementVector)
		{
			#if UNITY_EDITOR
			_playerTransform.Translate(movementVector * _playerSettings.PlayerSpeed * Time.deltaTime);
			#else
			_playerTransform.Translate(movementVector * _playerSpeed * Time.deltaTime);
			#endif
		}
	}
}

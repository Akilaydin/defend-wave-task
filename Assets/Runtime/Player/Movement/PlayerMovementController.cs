using Cysharp.Threading.Tasks.Linq;

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

		private Transform _playerTransform;
		
		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_inputService.MovementVector.WithoutCurrent().Subscribe(MovePlayer));
		}

		private void SetPlayerTransform(PlayerView playerView)
		{
		}

		private void MovePlayer(Vector2 movementVector)
		{
			_playerTransform.Translate(movementVector * 10 * Time.deltaTime);
		}
	}
}

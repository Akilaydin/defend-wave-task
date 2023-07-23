using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Input;

using OriGames.Extensions.Disposable;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.Player.Movement
{
	public class PlayerMovementController : Disposable, IInitializable
	{
		[Inject] private readonly IInputService _inputService;
		
		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_inputService.MovementVector.WithoutCurrent().Subscribe(MovePlayer));
		}

		private void MovePlayer(Vector2 movementVector)
		{
			Debug.Log(movementVector);
		}
	}
}

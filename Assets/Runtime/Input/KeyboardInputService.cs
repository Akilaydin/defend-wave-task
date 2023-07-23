using Cysharp.Threading.Tasks;

using UnityEngine;

using VContainer.Unity;

namespace DefendTheWave.Input
{
	public class KeyboardInputService : IInputService, ITickable
	{
		public IReadOnlyAsyncReactiveProperty<Vector2> MovementVector => _movementVector;

		private AsyncReactiveProperty<Vector2> _movementVector = new(Vector2.zero);

		void ITickable.Tick()
		{
			_movementVector.Value = new Vector2(
				UnityEngine.Input.GetAxisRaw(IInputService.HorizontalAxis),
				UnityEngine.Input.GetAxisRaw(IInputService.VerticalAxis))
				.normalized;
		}
	}
}

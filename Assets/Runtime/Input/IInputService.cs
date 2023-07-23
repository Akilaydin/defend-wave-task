using Cysharp.Threading.Tasks;

using UnityEngine;

namespace DefendTheWave.Input
{
	public interface IInputService
	{
		public const string HorizontalAxis = "Horizontal";
		public const string VerticalAxis = "Vertical";
		
		IReadOnlyAsyncReactiveProperty<Vector2> MovementVector { get; }
	}
}

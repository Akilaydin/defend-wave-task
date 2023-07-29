using DefendTheWave.Common.Services;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Common
{
	public class PlayerClampedPositionProvider : IStartable
	{
		[Inject] private readonly ScreenBoundsProvider _screenBoundsProvider;
		
		private Bounds _screenBounds;

		void IStartable.Start()
		{
			_screenBounds = _screenBoundsProvider.GetScreenBounds();
		}
		
		public Vector2 GetClampedPosition(float maxY, float offsetX, float offsetY, Vector2 sourcePosition)
		{
			sourcePosition.x = Mathf.Clamp(sourcePosition.x, _screenBounds.min.x + offsetX, _screenBounds.max.x - offsetX);
			sourcePosition.y = Mathf.Clamp(sourcePosition.y, _screenBounds.min.y + offsetY, maxY - offsetY);
			
			return sourcePosition;
		}
	}
}
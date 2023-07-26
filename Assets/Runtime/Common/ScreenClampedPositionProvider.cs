using DefendTheWave.Common.Services;
using DefendTheWave.Data;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Common
{
	public class ScreenClampedPositionProvider : IStartable
	{
		[Inject] private readonly ScreenBoundsProvider _screenBoundsProvider;
		
		private Bounds _screenBounds;

		void IStartable.Start()
		{
			_screenBounds = _screenBoundsProvider.GetScreenBounds();
		}
		
		private Vector3 GetClampedPosition(BoundsData boundsData, Vector3 sourcePosition)
		{
			var bounds = boundsData.Bounds;
			
			var rightOffset = Mathf.Abs(bounds.max.x - bounds.center.x);
			var leftOffset = Mathf.Abs(bounds.center.x - bounds.min.x);
			var topOffset = Mathf.Abs(bounds.max.y - bounds.center.y);
			var bottomOffset = Mathf.Abs(bounds.center.y - bounds.min.y);

			sourcePosition.x = Mathf.Clamp(sourcePosition.x, _screenBounds.min.x + leftOffset, _screenBounds.max.x - rightOffset);
			sourcePosition.y = Mathf.Clamp(sourcePosition.y, _screenBounds.min.y + bottomOffset, _screenBounds.max.y - topOffset);

			return sourcePosition;
		}
	}
}
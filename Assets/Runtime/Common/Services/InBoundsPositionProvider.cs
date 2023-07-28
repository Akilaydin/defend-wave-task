using DefendTheWave.Data;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.Common.Services
{
	public class InBoundsPositionProvider : IStartable
	{
		[Inject] private readonly ScreenBoundsProvider _screenBoundsProvider;
		
		private Bounds _screenBounds;

		void IStartable.Start()
		{
			_screenBounds = _screenBoundsProvider.GetScreenBounds();
		}
		
		public Vector3 GetClampedPosition(BoundsData boundsData, Vector3 sourcePosition)
		{
			var boundSize = boundsData.Bounds;
			
			sourcePosition.x = Mathf.Clamp(sourcePosition.x, boundSize.min.x, boundSize.max.x);
			sourcePosition.y = Mathf.Clamp(sourcePosition.y, boundSize.min.y, boundSize.max.y);
			
			return sourcePosition;
		}
	}
}

﻿using DefendTheWave.Common.Services;
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
		
		public Vector3 GetClampedPosition(BoundsData boundsData, Vector3 sourcePosition)
		{
			var boundSize = boundsData.Bounds;
			
			sourcePosition.x = Mathf.Clamp(sourcePosition.x, _screenBounds.min.x, _screenBounds.max.x);
			sourcePosition.y = Mathf.Clamp(sourcePosition.y, _screenBounds.min.y, boundSize.max.y);
			
			return sourcePosition;
		}
	}
}
using DefendTheWave.Data;

using UnityEngine;

using VContainer.Unity;

namespace DefendTheWave.Common.Services
{
	public class ScreenBoundsProvider : IInitializable
	{
		private readonly Camera _levelCamera;
		
		private Bounds _screenBounds;

		public ScreenBoundsProvider(LevelSceneData levelSceneData)
		{
			_levelCamera = levelSceneData.LevelCamera;
		}
		
		void IInitializable.Initialize()
		{
			CalculateScreenBounds();
		}
		
		public Bounds GetScreenBounds()
		{
			_screenBounds.center = _levelCamera.transform.position;
            
			return _screenBounds;
		}

		private void CalculateScreenBounds()
		{
			var leftBottomCorner = _levelCamera.ScreenToWorldPoint(Vector2.zero);
			var rightTopCorner = _levelCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

			var width = rightTopCorner.x - leftBottomCorner.x;
			var height = rightTopCorner.y - leftBottomCorner.y;
            
			_screenBounds = new Bounds(Vector3.zero, new Vector3(width, height, 0f));
		}
	}
}

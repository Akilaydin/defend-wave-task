using UnityEngine;

namespace DefendTheWave.Common
{
	public static class UIExtensions
	{
		public static void ResizeBySafeArea(this RectTransform transformToResize)
		{
			var safeArea = Screen.safeArea;

			var anchorMin = safeArea.position;
			var anchorMax = safeArea.position + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			transformToResize.anchorMin = anchorMin;
			transformToResize.anchorMax = anchorMax;
		}
	}
}
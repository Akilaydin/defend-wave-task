using UnityEngine;

namespace DefendTheWave.Common
{
	public class ResizerBySafeArea : MonoBehaviour
	{
		[SerializeField] private RectTransform _safeArea;

		#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_safeArea == null)
			{
				_safeArea = GetComponent<RectTransform>();
			}
		}
		#endif
        
		private void Awake()
		{
			_safeArea.ResizeBySafeArea();
		}
	}
}
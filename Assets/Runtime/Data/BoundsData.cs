using EasyButtons;

using UnityEngine;

namespace DefendTheWave.Data
{
	public class BoundsData : MonoBehaviour
	{
		public Bounds Bounds => _bounds;

		[SerializeField] private Bounds _bounds;

		private void Awake()
		{
			_bounds.center = (Vector2)transform.position;
		}

		[Button]
		private void SetAsCameraRect()
		{
			var mainCamera = Camera.main;

			var screenAspect = Screen.width / (float)Screen.height;
			var cameraHeight = mainCamera!.orthographicSize * 2;

			_bounds = new Bounds((Vector2)transform.position, new Vector3(cameraHeight * screenAspect * 0.586f, cameraHeight, 0));
		}

		private void OnDrawGizmosSelected()
		{
			_bounds.center = (Vector2)transform.position;

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(new Vector2(_bounds.min.x, _bounds.min.y), new Vector2(_bounds.min.x, _bounds.max.y));
			Gizmos.DrawLine(new Vector2(_bounds.max.x, _bounds.min.y), new Vector2(_bounds.max.x, _bounds.max.y));

			Gizmos.color = Color.red;
			Gizmos.DrawLine(new Vector2(_bounds.min.x, _bounds.min.y), new Vector2(_bounds.max.x, _bounds.min.y));
			Gizmos.DrawLine(new Vector2(_bounds.min.x, _bounds.max.y), new Vector2(_bounds.max.x, _bounds.max.y));
		}
	}
}

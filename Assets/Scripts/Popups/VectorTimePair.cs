using UnityEngine;

namespace FlappyCat.Popups
{
	[System.Serializable]
	public class VectorTimePair
	{
		[field: SerializeField]
		public float Time { get; private set; }
		
		[field: SerializeField]
		public Vector2 Vector { get; private set; }
	}
}

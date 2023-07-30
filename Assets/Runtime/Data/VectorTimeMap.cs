using UnityEngine;

namespace DefendTheWave.Data
{
	[System.Serializable]
	public class VectorTimeMap
	{
		[field: SerializeField] public Vector2 Vector { get; private set; }
		[field: SerializeField] public float Time { get; private set; }
	}
}

using UnityEngine;

namespace DefendTheWave.Common
{
	public static class DefendTheWaveExtensions
	{
		public static float GetRandomFromVector(this Vector2 vector2)
		{
			return Random.Range(vector2.x, vector2.y);
		}
		
		public static bool Includes(this LayerMask mask, int layer)
		{
			return (mask.value & 1 << layer) > 0;
		}
	}
}

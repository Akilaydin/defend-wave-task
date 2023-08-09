using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

namespace DefendTheWave.Common
{
	public static class DefendTheWaveExtensions
	{
		public static float GetRandomFromVector(this Vector2 vector2)
		{
			return Random.Range(vector2.x, vector2.y);
		}
		
		public static T GetRandomElement<T>(this IEnumerable<T> source)
		{
			return source.Shuffle().First();
		}

		public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> source, int count)
		{
			return source.Shuffle().Take(count);
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(x => Guid.NewGuid());
		}
		
		public static bool Includes(this LayerMask mask, int layer)
		{
			return (mask.value & 1 << layer) > 0;
		}
	}
}

using System;

using UnityEngine;

namespace DefendTheWave.Data.Settings
{
	[Serializable]
	public class EnemySettings : ICloneable
	{
		[field: SerializeField, MinMaxSlider(0.01f, 10f)] public Vector2 Speed { get; private set; } = new(5f, 10f);
		[field: SerializeField] public int Health { get; private set; } = 3;
		[field: SerializeField] public int DamageOnLeak { get; private set; } = 1;

		public object Clone()
		{
			return (EnemySettings) MemberwiseClone();
		}
	}
}

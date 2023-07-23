using System;

using UnityEngine;

namespace DefendTheWave.Data.Settings
{
	[Serializable]
	public class EnemySettings : ICloneable
	{
		[field: SerializeField, MinMaxSlider(1f, 99.9f)] public Vector2 Speed { get; private set; } = new(5f, 10f);
		[field: SerializeField] public int Health { get; private set; } = 3;
		[field: SerializeField] public int DamageOnLeak { get; private set; } = 1; //note: В ТЗ зашито 1, но решил дать возможность настройки

		public object Clone()
		{
			return (EnemySettings) MemberwiseClone();
		}
	}
}

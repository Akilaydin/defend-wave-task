using DefendTheWave.Common;
using DefendTheWave.Data.Settings;

using UnityEngine;

namespace DefendTheWave.Data
{
	[CreateAssetMenu(fileName = nameof(EnemiesPack), menuName = GameConstants.ScriptableObjectsRoot + nameof(EnemiesPack), order = 0)]
	public class EnemiesPack : ScriptableObject
	{
		[field: SerializeField] public SpawnableEnemySettings[] Enemies { get; private set; }
	}
}

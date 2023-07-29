using DefendTheWave.Player.Health;

using UnityEngine;

namespace DefendTheWave.Data
{
	public class LevelSceneData : MonoBehaviour
	{
		[field: SerializeField] public Camera LevelCamera { get; private set; }
		[field: SerializeField] public Transform PlayerSpawnRoot { get; private set; }
		[field: SerializeField] public BoundsData PlayerBounds { get; private set; }
		[field: SerializeField] public PlayerHealthView PlayerHealthView { get; private set; }
		[field: SerializeField] public Transform[] EnemiesSpawnPoints { get; private set; }
	}
}

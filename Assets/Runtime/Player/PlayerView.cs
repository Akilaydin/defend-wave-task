using DefendTheWave.Common.Services.Spawn;

using UnityEngine;

namespace DefendTheWave.Player
{
	public class PlayerView : MonoBehaviour, ISpawnableEntity
	{
		[field: SerializeField] public SpriteRenderer Renderer { get; private set; }
		
		public Transform PlayerTransform { get; private set; }
		
		void ISpawnableEntity.OnSpawned()
		{
			PlayerTransform = transform;
		}
	}
}

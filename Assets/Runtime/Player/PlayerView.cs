using DefendTheWave.Common.Services.Spawn;

using UnityEngine;

namespace DefendTheWave.Player
{
	public class PlayerView : MonoBehaviour, ISpawnableEntity
	{
		public Transform PlayerTransform { get; private set; }
		
		void ISpawnableEntity.OnSpawned()
		{
			PlayerTransform = transform;
		}
	}
}

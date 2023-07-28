using DefendTheWave.Common.Services.Spawn.Pooling;

using UnityEngine;

namespace DefendTheWave.Player.Shooting
{
	public class BulletView : MonoBehaviour, IPoolableObject
	{
		public void OnCreated() { }

		public void OnGotFromPool() { }

		public void OnReturnedToPool() { }

		public void OnDestroyed() { }
	}
}

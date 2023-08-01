using DefendTheWave.Common.Services.Spawn;
using DefendTheWave.Common.Services.Spawn.Pooling;

using UnityEngine;

namespace Runtime.Enemies
{
	public class EnemyView : MonoBehaviour, IPoolableObject
	{
		void IPoolableObject.OnCreated() { }

		void IPoolableObject.OnGotFromPool() { }

		void IPoolableObject.OnReturnedToPool() { }

		void IPoolableObject.OnDestroyed() { }

		void ISpawnableEntity.OnSpawned() { }
	}
}

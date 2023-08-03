using System;

using UnityEngine;

namespace DefendTheWave.Enemies
{
	public abstract class BaseEnemyView : MonoBehaviour
	{
		public event Action<Collider2D> OnTriggerEnterEvent;

		private void OnTriggerEnter2D(Collider2D sourceCollider)
		{
			OnTriggerEnterEvent?.Invoke(sourceCollider);
		}
	}
}
using System;

using UnityEngine;

namespace DefendTheWave.Player.Shooting
{
	public class BulletView : MonoBehaviour
	{
		public event Action HitEnemy;

		public void InvokeHitEnemy()
		{
			HitEnemy?.Invoke();
		}
	}
}

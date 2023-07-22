using UnityEngine;

using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class GameEntryPoint : MonoBehaviour
	{
		[SerializeField] private LifetimeScope _initialLifetimeScope;

		private void Awake()
		{
			_initialLifetimeScope.Build();
		}
	}
}

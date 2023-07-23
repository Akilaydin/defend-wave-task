using UnityEngine;

using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class GameEntryPoint : MonoBehaviour
	{
		[SerializeField] private LifetimeScope _bootstrapLifetimeScope;
		[SerializeField] private LifetimeScope _levelLifetimeScope;

		private void Awake()
		{
			_bootstrapLifetimeScope.Build();

			using (LifetimeScope.EnqueueParent(_bootstrapLifetimeScope))
			{
				_levelLifetimeScope.Build();
			}
		}
	}
}

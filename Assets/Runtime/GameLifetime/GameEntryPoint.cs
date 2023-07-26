using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class GameEntryPoint : MonoBehaviour
	{
		[SerializeField] private LifetimeScope _bootstrapLifetimeScope;
		[SerializeField] private LifetimeScope _levelLifetimeScope;

		private void Awake()
		{
			InitializeGameAsync().Forget();
		}

		private async UniTaskVoid InitializeGameAsync()
		{
			await Addressables.InitializeAsync();
			
			_bootstrapLifetimeScope.Build();

			using (LifetimeScope.EnqueueParent(_bootstrapLifetimeScope))
			{
				_levelLifetimeScope.Build();
			}
		}
	}
}

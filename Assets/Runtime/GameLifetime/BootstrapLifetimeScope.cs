using Cysharp.Threading.Tasks;

using DefendTheWave.Input;

using UnityEngine.AddressableAssets;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class BootstrapLifetimeScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			Addressables.InitializeAsync().ToUniTask().Forget();

			builder.Register<KeyboardInputService>(Lifetime.Singleton).AsImplementedInterfaces();
		}
	}
}

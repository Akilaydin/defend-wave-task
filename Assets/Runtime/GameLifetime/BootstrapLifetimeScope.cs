using DefendTheWave.Common;
using DefendTheWave.Common.Services;
using DefendTheWave.Input;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class BootstrapLifetimeScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<KeyboardInputService>(Lifetime.Singleton).AsImplementedInterfaces();
			
			builder.Register<ScreenBoundsProvider>(Lifetime.Singleton).AsSelf();
			builder.Register<ClampedPositionProvider>(Lifetime.Singleton).AsSelf();
		}
	}
}

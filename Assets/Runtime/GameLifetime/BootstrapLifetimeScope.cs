using VContainer;
using VContainer.Unity;

namespace DefendTheWave.GameLifetime
{
	public class BootstrapLifetimeScope : LifetimeScope
	{
		private LifetimeScope _levelLifetimeScope;
		
		protected override void Configure(IContainerBuilder builder)
		{
			_levelLifetimeScope = CreateLevelLifetimeScope();
				
			LifetimeScope CreateLevelLifetimeScope()
			{
				return CreateChild(levelScopeBuilder =>
				{
					
				});
			}
		}
	}
}

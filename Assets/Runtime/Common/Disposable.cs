using System;

using OriGames.Extensions.Disposable;

namespace DefendTheWave.Common
{
	public class Disposable : IDisposable
	{
		protected CompositeDisposable CompositeDisposable = new();

		void IDisposable.Dispose()
		{
			CompositeDisposable.Dispose();
		}
	}
}

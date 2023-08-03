﻿using System;

using OriGames.Extensions.Disposable;

namespace DefendTheWave.Common
{
	public class Disposable : IDisposable
	{
		protected readonly CompositeDisposable CompositeDisposable = new();

		void IDisposable.Dispose()
		{
			CompositeDisposable.Dispose();
			
			OnDispose();
		}

		protected virtual void OnDispose() { }
	}
}

using System;

using UnityEngine;

namespace DefendTheWave.Common.Services.Spawn.Pooling.AddressablesPooling
{
	public sealed class PooledObject : IDisposable
	{
		public PooledObject(AddressablePool pool, GameObject instance)
		{
			Id = Guid.NewGuid().ToString();
			Pool = pool;
			Instance = instance;
		}

		public bool IsDisposed { get; private set; }

		public string Id { get; }
		private AddressablePool Pool { get; }
		public GameObject Instance { get; }

		public void Dispose()
		{
			if (IsDisposed)
				return;

			Pool.Return(this);
			IsDisposed = true;
		}
	}
}

using System.Threading;

using Cysharp.Threading.Tasks;

namespace DefendTheWave.Common.Services.Spawn.Pooling
{
	public interface IAsyncObjectPool<T> where T : IPoolableObject
	{
		UniTask<T> GetAsync(CancellationToken tokenCancellationToken );

		void ReturnToPool(T poolableObject);
	}
}

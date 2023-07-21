using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

using VContainer;

namespace FlappyCat.Popups
{
	public abstract class BaseAppearanceBehaviour
	{
		protected static Transform TargetTransform;
		
		public void SetTargetTransform(Transform targetTransform)
		{
			TargetTransform = targetTransform;
		}

		public abstract void ResolveSettings(IObjectResolver resolver);

		public abstract UniTask ShowAsync(CancellationToken token);

		public abstract UniTask HideAsync(CancellationToken token);
	}
}

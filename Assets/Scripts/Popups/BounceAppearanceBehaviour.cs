using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using OriGames.Extensions.TweenExtensions;

using VContainer;

namespace FlappyCat.Popups
{
	public class BounceAppearanceBehaviour : BaseAppearanceBehaviour
	{
		private BounceAppearanceSettings _bounceAppearanceSettings;

		public override void ResolveSettings(IObjectResolver resolver)
		{
			_bounceAppearanceSettings = resolver.Resolve<BounceAppearanceSettings>();
		}

		public override async UniTask ShowAsync(CancellationToken token)
		{
			await ScaleAsync(_bounceAppearanceSettings.AppearanceVectorTimePairs, _bounceAppearanceSettings.AppearanceEaseType, token);
		}

		public override async UniTask HideAsync(CancellationToken token)
		{
			await ScaleAsync(_bounceAppearanceSettings.HidingVectorTimePairs, _bounceAppearanceSettings.HidingEaseType, token);
		}

		private async UniTask ScaleAsync(VectorTimePair[] vectorTimePairs, Ease easeType, CancellationToken token)
		{
			foreach (var vectorTimePair in vectorTimePairs)
			{
				await TargetTransform.DOScale(vectorTimePair.Vector, vectorTimePair.Time).SetEase(easeType).AwaitWithCancellation(token);
			}
		}
	}
}

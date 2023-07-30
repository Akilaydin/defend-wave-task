using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DefendTheWave.Data;

using DG.Tweening;

using UnityEngine;

namespace DefendTheWave.GUI
{
	public abstract class BasePopupView : MonoBehaviour, IDisposable
	{
		[SerializeField] private GameObject _popupGameObject;
		[SerializeField] private Transform _popupTransform;
		[SerializeField] private PopupBounceConfiguration _showConfiguration;
		[SerializeField] private PopupBounceConfiguration _hideConfiguration;

		private Tween _lastTween;
		
		public async UniTask ShowAsync(CancellationToken token)
		{
			_popupGameObject.SetActive(true);

			await ScalePopup(_showConfiguration, token);
		}
		
		public async UniTask HideAsync(CancellationToken token)
		{
			await ScalePopup(_hideConfiguration, token);
			
			_popupGameObject.SetActive(false);
		}

		private async UniTask ScalePopup(PopupBounceConfiguration bounceConfiguration, CancellationToken token)
		{
			var sequence = DOTween.Sequence();

			foreach (var vectorTimeMap in bounceConfiguration.VectorTimeMaps)
			{
				sequence.Append(_popupTransform.DOScale(vectorTimeMap.Vector, vectorTimeMap.Time));
			}

			_lastTween = sequence;

			await _lastTween.ToUniTask(cancellationToken: token);
		}

		void IDisposable.Dispose()
		{
			if (_lastTween.IsActive())
			{
				_lastTween.Kill();
			}
		}
	}
}

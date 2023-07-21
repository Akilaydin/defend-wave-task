using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Assertions;

using VContainer;

namespace FlappyCat.Popups
{
	public class BasePopup : MonoBehaviour
	{
		[SerializeReference, ReferencePicker]
		private BaseAppearanceBehaviour _appearanceBehaviour;

		[SerializeField]
		private Transform _popupMainTransform;
		
		[SerializeField]
		private Transform _contentAppearanceTransform;

		private bool _initialized;

		public void Initialize(IObjectResolver resolver)
		{
			Assert.IsFalse(_initialized, "tried to initialize popup twice");

			_appearanceBehaviour.SetTargetTransform(_contentAppearanceTransform);
			_appearanceBehaviour.ResolveSettings(resolver);

			_initialized = true;
		}
		
		public async UniTask ShowPopupAsync(CancellationToken token)
		{
			_popupMainTransform.gameObject.SetActive(true);
			
			await _appearanceBehaviour.ShowAsync(token);
		}

		public async UniTask HidePopupAsync(CancellationToken token)
		{
			await _appearanceBehaviour.HideAsync(token);
			
			_popupMainTransform.gameObject.SetActive(false);
		}
	}
}

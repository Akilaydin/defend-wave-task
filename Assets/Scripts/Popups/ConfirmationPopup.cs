using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace FlappyCat.Popups
{
	public class ConfirmationPopup : BasePopup
	{
		[SerializeField]
		private Button _yesButton;

		[SerializeField]
		private Button _noButton;

		public async UniTask<bool> AwaitForConfirmation()
		{
			var yesTask = _yesButton.OnClickAsync();
			var noTask = _noButton.OnClickAsync();

			await UniTask.WhenAny(yesTask, noTask);

			return yesTask.Status == UniTaskStatus.Succeeded;
		}
	}
}

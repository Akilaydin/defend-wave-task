using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common;
using DefendTheWave.GameLifetime;
using DefendTheWave.GameLifetime.Interfaces;
using DefendTheWave.GUI.Views;

using VContainer;
using VContainer.Unity;

namespace DefendTheWave.GUI.Controllers
{
	public class LostPopupController : Disposable, IInitializable
	{
		[Inject] private readonly LostPopupView _lostPopupView;
		[Inject] private readonly IGameStateProvider _gameStateProvider;
		[Inject] private readonly GameRestarter _gameRestarter;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_gameStateProvider.CurrentGameState.Subscribe(HandleGameStateChanged));
		}

		private void HandleGameStateChanged(GameState newGameState)
		{
			if (newGameState == GameState.Lost)
			{
				_lostPopupView.ShowAsync(CancellationToken.None).ContinueWith(() =>
				{
					CompositeDisposable.Add(_lostPopupView.RestartButton.OnClickAsAsyncEnumerable().Subscribe(HandleRestartClicked));
				}).Forget();
			}
		}

		private void HandleRestartClicked(AsyncUnit _)
		{
			_gameRestarter.RestartGame();
		}
	}
}

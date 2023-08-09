

using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.GameLifetime;
using DefendTheWave.GameLifetime.Interfaces;
using DefendTheWave.GUI.Views;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.GUI.Controllers
{
	public class VictoryPopupController : Disposable, IInitializable
	{
		[Inject] private readonly VictoryPopupView _victoryPopupView;
		[Inject] private readonly IGameStateProvider _gameStateProvider;
		[Inject] private readonly GameRestarter _gameRestarter;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_gameStateProvider.CurrentGameState.Subscribe(HandleGameStateChanged));
		}

		private void HandleGameStateChanged(GameState newGameState)
		{
			if (newGameState == GameState.Won)
			{
				_victoryPopupView.ShowAsync(CancellationToken.None).ContinueWith(() =>
				{
					CompositeDisposable.Add(_victoryPopupView.ContinueButton.OnClickAsAsyncEnumerable().Subscribe(HandleContinueClicked));
				}).Forget();
			}
		}

		private void HandleContinueClicked(AsyncUnit _)
		{
			_gameRestarter.RestartGame();
		}
	}
}

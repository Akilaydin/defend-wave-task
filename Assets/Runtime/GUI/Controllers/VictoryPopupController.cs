

using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.GameLifetime;
using DefendTheWave.GUI.Views;

using OriGames.Extensions.Disposable;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.GUI.Controllers
{
	public class VictoryPopupController : Disposable, IInitializable
	{
		[Inject] private readonly VictoryPopupView _victoryPopupView;
		[Inject] private readonly GameStateModel _gameStateModel;
		[Inject] private readonly GameRestarter _gameRestarter;

		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_gameStateModel.CurrentGameState.Subscribe(HandleGameStateChanged));
		}

		private void HandleGameStateChanged(GameState newGameState)
		{
			if (newGameState == GameState.Won)
			{
				CompositeDisposable.Add(_victoryPopupView.ContinueButton.OnClickAsAsyncEnumerable().Subscribe(HandleContinueClicked));
				
				_victoryPopupView.ShowAsync(CancellationToken.None).Forget();
			}
		}

		private void HandleContinueClicked(AsyncUnit _)
		{
			_gameRestarter.RestartGame();
		}
	}
}

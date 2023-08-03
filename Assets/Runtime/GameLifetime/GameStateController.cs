using System;
using System.Collections.Generic;
using System.Linq;

using DefendTheWave.Common;
using DefendTheWave.Data.Settings;
using DefendTheWave.GameLifetime.Interfaces;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using IGameEndedHandler = DefendTheWave.GameLifetime.Interfaces.IGameEndedHandler;

namespace DefendTheWave.GameLifetime
{
	public class GameStateController : IStartable, IDisposable
	{
		[Inject] private readonly IGameStateChanger _gameStateChanger;
		[Inject] private readonly GlobalGameSettings _gameSettings;
		[Inject] private readonly IGameEventsProvider _gameEventBus;
		[Inject] private readonly IEnumerable<IGameEndedHandler> _gameEndedHandlers;
		[Inject] private readonly IObjectResolver _resolver;

		private int _destroyedEnemiesToWin;
		
		private int _damageToLose;
		private int _currentDestroyedEnemiesCount;

		void IStartable.Start()
		{
			_destroyedEnemiesToWin = (int) _gameSettings.DefeatedEnemiesCountToWin.GetRandomFromVector();
			
			_damageToLose = _gameSettings.DamageToLose;
			
			_gameEventBus.EnemyLeaked += HandleEnemyLeaked;
			_gameEventBus.EnemyDestroyed += HandleEnemyDestroyed;
		}
        
		void IDisposable.Dispose()
		{
			_gameEventBus.EnemyLeaked -= HandleEnemyLeaked;
			_gameEventBus.EnemyDestroyed -= HandleEnemyDestroyed;
		}

		private void HandleEnemyDestroyed()
		{
			_currentDestroyedEnemiesCount++;

			if (_currentDestroyedEnemiesCount >= _destroyedEnemiesToWin)
			{
				_gameStateChanger.ChangeGameState(GameState.Won);
				
				InvokeGameEndedHandlers();
			}
		}

		private void HandleEnemyLeaked(int enemyDamageOnLeak)
		{
			_damageToLose -= enemyDamageOnLeak;

			if (_damageToLose <= 0)
			{
				_gameStateChanger.ChangeGameState(GameState.Lost);
				
				InvokeGameEndedHandlers();
			}
		}

		private void InvokeGameEndedHandlers()
		{
			foreach (var gameEndedHandler in _gameEndedHandlers)
			{
				gameEndedHandler.HandleGameEnded();
			}
		}
	}
}

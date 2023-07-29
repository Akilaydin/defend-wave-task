using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Input;
using DefendTheWave.GameEnvironment;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.Player.Movement
{
	public class PlayerMovementController : Disposable, IInitializable, IStartable
	{
		[Inject] private readonly IInputService _inputService;
		[Inject] private readonly PlayerContainer _playerContainer;
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;
		[Inject] private readonly PlayerClampedPositionProvider _playerClampedPositionProvider;
		[Inject] private readonly LevelSceneData _levelSceneData;

		private FinishLineView _finishLineView;
		private Transform _playerTransform;
		
		private float _movementOffsetX;
		private float _movementOffsetY;
		private float _maxYMovement;
		
		private float _playerSpeed;
		
		void IInitializable.Initialize()
		{
			_playerSpeed = _playerSettings.PlayerSpeed;
			_finishLineView = _levelSceneData.FinishLineView;
			
			_maxYMovement = _finishLineView.transform.position.y - _finishLineView.Renderer.bounds.size.y / 2;
		}

		void IStartable.Start()
		{
			CompositeDisposable.Add(_inputService.MovementVector.WithoutCurrent().Subscribe(AdjustPlayerPosition));
			CompositeDisposable.Add(_playerContainer.Player.Subscribe(CachePlayerData));
		}
		
		// ReSharper disable once Unity.InefficientPropertyAccess
		private void CachePlayerData(PlayerView playerView)
		{
			_playerTransform = playerView.transform;

			_movementOffsetX = playerView.Renderer.sprite.bounds.size.x / 2;
			_movementOffsetY = playerView.Renderer.sprite.bounds.size.y / 2;
		}

		private void AdjustPlayerPosition(Vector2 movementVector)
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			Vector2 newPosition;
				
			#if UNITY_EDITOR
			newPosition = (Vector2)_playerTransform.position +movementVector * _playerSettings.PlayerSpeed * Time.deltaTime;
			#else
			newPosition = (Vector2)_playerTransform.position + movementVector * _playerSpeed * Time.deltaTime;
			#endif
			
			var clampedPosition = _playerClampedPositionProvider.GetClampedPosition(_maxYMovement, _movementOffsetX, _movementOffsetY, newPosition);

			_playerTransform.position = clampedPosition;
		}
	}
}

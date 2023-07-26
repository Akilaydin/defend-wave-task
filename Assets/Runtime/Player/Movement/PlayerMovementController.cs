using Cysharp.Threading.Tasks.Linq;

using DefendTheWave.Common;
using DefendTheWave.Data;
using DefendTheWave.Data.Settings;
using DefendTheWave.Input;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Disposable = DefendTheWave.Common.Disposable;

namespace DefendTheWave.Player.Movement
{
	public class PlayerMovementController : Disposable, IInitializable
	{
		[Inject] private readonly IInputService _inputService;
		[Inject] private readonly PlayerContainer _playerContainer;
		[Inject] private readonly SpawnablePlayerSettings _playerSettings;
		[Inject] private readonly ScreenClampedPositionProvider _clampedPositionProvider;
		[Inject] private readonly LevelSceneData _levelSceneData;

		private Transform _playerTransform;
		private BoundsData _playerBounds;
		
		private float _playerSpeed;
		
		void IInitializable.Initialize()
		{
			CompositeDisposable.Add(_inputService.MovementVector.WithoutCurrent().Subscribe(AdjustPlayerPosition));
			CompositeDisposable.Add(_playerContainer.Player.Subscribe(CachePlayerTransform));

			_playerSpeed = _playerSettings.PlayerSpeed;
			_playerBounds = _levelSceneData.PlayerBounds;
		}

		private void CachePlayerTransform(PlayerView playerView)
		{
			_playerTransform = playerView.transform;
		}

		private void AdjustPlayerPosition(Vector2 movementVector)
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			Vector2 newPosition;
				
			#if UNITY_EDITOR
			newPosition = (Vector2)_playerTransform.position +
				movementVector * _playerSettings.PlayerSpeed * Time.deltaTime;
			#else
			newPosition = (Vector2)_playerTransform.position +
				movementVector * _playerSpeed * Time.deltaTime;
			#endif

			Vector2 clampedPosition = _clampedPositionProvider.GetClampedPosition(_playerBounds, newPosition);
			
			_playerTransform.position = clampedPosition;
		}
	}
}

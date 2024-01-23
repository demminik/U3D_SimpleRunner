using Runner.Gameplay.Core.Levels;
using Runner.Gameplay.Core.Tiles;
using UnityEngine;

namespace Runner.Gameplay.Core.Movement {

    /// <summary>
    /// Script to move runner along the level splines
    /// </summary>
    public class SplineMovementManager : IMovementManager {

        private struct SplineInfo {

            public int CurrentIndex;
            public float Length;
            public float PercentsTraveled;
            public float DistanceTraveled;
        }

        private RunnerGameplayFasade _gameplayFasade;
        private IRunningObject _runner;
        private ILevelLogic _levelLogic;

        private Tile _currentTile;
        private bool _isCurrentTileValid = false;
        private Tile CurrentTile {
            get => _currentTile;
            set {
                _currentTile = value;
                _isCurrentTileValid = _currentTile != null;
            }
        }

        private SplineInfo _currentSplineInfo;

        public SplineMovementManager(RunnerGameplayFasade gameplayFasade, IRunningObject runner, ILevelLogic levelDataProvider) {
            Initialize(gameplayFasade, runner, levelDataProvider);
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade, IRunningObject runner, ILevelLogic levelDataProvider) {
            _gameplayFasade = gameplayFasade;
            _runner = runner;
            _levelLogic = levelDataProvider;

            SubscribeToPlayerInput();
        }

        public void Dispose() {
            UnsubscribeFromPlayerInput();

            _runner = null;
            _levelLogic = null;
            _gameplayFasade = null;
        }

        public void StartExecution() {
            const int startTileIndex = 0;
            CurrentTile = _levelLogic.GetTile(startTileIndex);

            var startSplineIndex = (int)(CurrentTile.Splines.Splines.Count / 2f);
            _currentSplineInfo = new SplineInfo() {
                CurrentIndex = startSplineIndex,
                Length = CurrentTile.Splines.CalculateLength(startSplineIndex),
                PercentsTraveled = 0f,
                DistanceTraveled = 0f,
            };

            CalculatePositionAndRotationOnSpline(_currentSplineInfo.CurrentIndex, 0f, out var position, out var rotation);
            _runner.SetStartTransform(position, rotation);
        }

        public void Tick(float deltaTime) {
            if (_runner.IsRunning && _isCurrentTileValid) {
                var splineDistanceTraveled = _currentSplineInfo.DistanceTraveled + _runner.CurrentSpeed * deltaTime;
                var splinePercentsTraveled = _currentSplineInfo.DistanceTraveled / _currentSplineInfo.Length;

                if (_currentSplineInfo.PercentsTraveled >= 1f) {
                    SwitchToNextTile();

                    // transfer excessive distance travelled to new tile
                    splineDistanceTraveled -= _currentSplineInfo.Length - _currentSplineInfo.DistanceTraveled;
                    splinePercentsTraveled -= 1f;
                }

                _currentSplineInfo.DistanceTraveled = splineDistanceTraveled;
                _currentSplineInfo.PercentsTraveled = splinePercentsTraveled;

                if (_isCurrentTileValid) {

                    ProcessSingleSplineMovement(deltaTime);

                    var shouldSwitchSplines = false;
                    if (!shouldSwitchSplines) {
                        ProcessSingleSplineMovement(deltaTime);
                    } else {
                        ProcessSwitchSplineMovement(deltaTime);
                    }
                }
            }
        }

        private void ProcessSingleSplineMovement(float deltaTime) {
            CalculatePositionAndRotationOnSpline(_currentSplineInfo.CurrentIndex, _currentSplineInfo.PercentsTraveled, out var newPosition, out var newRotation);

            _runner.MoveAndRotate(newPosition, newRotation);
            _levelLogic.UpdateRunnerLocation(CurrentTile.RuntimeIndex);
        }

        private void ProcessSwitchSplineMovement(float deltaTime) {
            // TODO: switch splines
        }

        private void CalculatePositionAndRotationOnSpline(int splineIndex, float t, out Vector3 outPosition, out Quaternion outRotation) {
            CurrentTile.Splines.Evaluate(splineIndex, t, out var position, out var tangent, out var upVector);

            outPosition = position;
            outRotation = Quaternion.LookRotation(tangent, upVector);
        }

        private void SwitchToNextTile() {
            var nextTileIndex = CurrentTile.RuntimeIndex + 1;
            CurrentTile = _levelLogic.GetTile(nextTileIndex);

            if (_isCurrentTileValid) {
                var newSplineIndex = _currentSplineInfo.CurrentIndex;
                _currentSplineInfo = new SplineInfo() {
                    CurrentIndex = newSplineIndex,
                    Length = CurrentTile.Splines.CalculateLength(newSplineIndex),
                    PercentsTraveled = 0f,
                    DistanceTraveled = 0f,
                };
            }
        }

        private void SubscribeToPlayerInput() {
            UnsubscribeFromPlayerInput();

            if (_gameplayFasade != null) {
                _gameplayFasade.PlayerInput.OnMoveRight += ProcessInputMoveRight;
                _gameplayFasade.PlayerInput.OnMoveLeft += ProcessInputMoveLeft;
                _gameplayFasade.PlayerInput.OnJump += ProcessInputJump;
            }
        }

        private void UnsubscribeFromPlayerInput() {
            if (_gameplayFasade != null) {
                _gameplayFasade.PlayerInput.OnMoveRight -= ProcessInputMoveRight;
                _gameplayFasade.PlayerInput.OnMoveLeft -= ProcessInputMoveLeft;
                _gameplayFasade.PlayerInput.OnJump -= ProcessInputJump;
            }
        }

        private void ProcessInputMoveRight() {
            if (_isCurrentTileValid && _currentSplineInfo.CurrentIndex < _currentTile.Splines.Splines.Count - 1) {
                _currentSplineInfo.CurrentIndex++;
            }
        }

        private void ProcessInputMoveLeft() {
            if (_isCurrentTileValid && _currentSplineInfo.CurrentIndex > 0) {
                _currentSplineInfo.CurrentIndex--;
            }
        }

        private void ProcessInputJump() {
            // TODO: implement jump
        }
    }
}
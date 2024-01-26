using Runner.Gameplay.Core;
using Runner.Gameplay.Core.Camera;
using Runner.Gameplay.Core.Characters;
using Runner.Gameplay.Core.Input;
using Runner.Gameplay.Core.Interacions;
using Runner.Gameplay.Core.Interactions.Coins;
using Runner.Gameplay.Core.Interactions.Obstacles;
using Runner.Gameplay.Core.Levels;
using Runner.Gameplay.Core.Movement;
using Runner.Gameplay.Core.Player;
using Runner.Gameplay.Core.Pooling;
using Runner.Gameplay.Core.Settings;
using Runner.Gameplay.Core.Timers;
using UnityEngine;

namespace Runner.Gameplay {

    public class GameplayModeDirector : MonoBehaviour {

        [SerializeField]
        private PrefabGameplaySettingsProvider _settingsProvider;

        [SerializeField]
        private PlayerInputProviderManager _inputProvider;

        [SerializeField]
        private GameplayPoolProvider _poolProvider;

        // TODO: move to some level-related stuff
        [SerializeField]
        private Transform _tilesRoot;

        // TODO: move to some level-related stuff
        [SerializeField]
        private Transform _characterRoot;

        // TODO: move to some level-related stuff
        [SerializeField]
        private RunnerCamera _mainCamera;

        // TODO: move to some time-related stuff
        [SerializeField]
        private TickProviderDefault _generalTickProvider;

        private GameTime _gameTime;

        private PlayerScore _playerScore;

        private RunnerGameplayFasade _gameplayFasade;
        private LevelBuilder _levelBuilder;
        private Character _currentCharacter;
        private IMovementManager _movementManager;

        private bool _isActiveAndRunning = false;

        private void Start() {
            CreateGameTime();

            CreatePlayerScore();
            var interactionProcessor = CreateInteractionProcessor();
            _poolProvider.Initialize(_settingsProvider);

            _gameplayFasade = new RunnerGameplayFasade(_gameTime, _settingsProvider, _inputProvider, _poolProvider, interactionProcessor);

            CreateLevel();
            CreateCharacter();
            CreateMovementManager();
            BindCamera();

            StartExecution();
        }

        private void OnDestroy() {
            Dispose();
        }

        private void Dispose() {
            if(_gameTime != null) {
                _gameTime.Dispose();
                _gameTime = null;
            }

            if (_poolProvider != null) {
                _poolProvider.Dispose();
            }

            if (_levelBuilder != null) {
                _levelBuilder.Dispose();
                _levelBuilder = null;
            }

            if (_currentCharacter != null) {
                Destroy(_currentCharacter.gameObject);
                _currentCharacter = null;
            }

            if (_movementManager != null) {
                _movementManager.Dispose();
                _movementManager = null;
            }

            if (_gameplayFasade != null) {
                _gameplayFasade.Dispose();
                _gameplayFasade = null;
            }

            if(_inputProvider != null) {
                _inputProvider.Dispose();
            }

            _isActiveAndRunning = false;
        }

        private void Update() {
            if (!_isActiveAndRunning) {
                return;
            }

            // TODO: bind to tick manager?
            _currentCharacter.Tick(Time.deltaTime);
            _movementManager.Tick(Time.deltaTime);
        }

        private void LateUpdate() {
            if (!_isActiveAndRunning) {
                return;
            }

            // TODO: bind to tick manager?
            _mainCamera.Tick(Time.deltaTime);
        }

        private void CreateGameTime() {
            // this is the place to create different time flow speed
            var generalTickManager = new TickManager(_generalTickProvider);

            _gameTime = new GameTime(generalTickManager);
        }

        private void CreatePlayerScore() {
            _playerScore = new PlayerScore();
        }

        private IInteractionProcessor CreateInteractionProcessor() {
            var coinInteractions = new CoinInteractionLogic(_settingsProvider, _playerScore);
            var obstacleInteractions = new ObstacleInteractionLogic();

            var result = new CommonInteractionProcessor(coinInteractions, obstacleInteractions);
            return result;
        }

        private void CreateLevel() {
            _levelBuilder = new LevelBuilder(_gameplayFasade, _tilesRoot);
            _levelBuilder.StartExecution();
        }

        private void CreateCharacter() {
            _currentCharacter = Instantiate(_gameplayFasade.Settings.Character.CharacterPrefab.gameObject, _characterRoot).GetComponent<Character>();
            _currentCharacter.Initialize(_gameplayFasade);
            _currentCharacter.OnDeath += ProcessCharacterDeath;
        }

        private void CreateMovementManager() {
            _movementManager = new SplineMovementManager(_gameplayFasade, _currentCharacter.RunningObject, _levelBuilder);
        }

        private void BindCamera() {
            _mainCamera.SetTarget(_currentCharacter.transform);
        }

        private void StartExecution() {
            _gameTime.StartExecution();
            _levelBuilder.StartExecution();
            _currentCharacter.StartExecution();
            _movementManager.StartExecution();
            _inputProvider.StartExecution();

            _isActiveAndRunning = true;
        }

        private void ProcessCharacterDeath() {
            Dispose();
            Start();
        }
    }
}
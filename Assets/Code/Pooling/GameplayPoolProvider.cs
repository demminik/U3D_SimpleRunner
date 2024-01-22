using Runner.Gameplay.Core.Obstacles;
using Runner.Gameplay.Core.Settings;
using UnityEngine;

namespace Runner.Gameplay.Core.Pooling {

    public class GameplayPoolProvider : MonoBehaviour, IGameplayPoolProvider {

        [SerializeField]
        private Transform _coinsRoot;

        [SerializeField]
        private Transform _obstaclesRoot;

        private IGameplaySettingsProvider _gameplaySettings;

        public CoinsPool Coins { get; private set; }

        public ObstaclesPool Obstacles { get; private set; }

        public void Initialize(IGameplaySettingsProvider gameplaySettings) {
            _gameplaySettings = gameplaySettings;

            Coins = new CoinsPool(_coinsRoot, _gameplaySettings.Coin);
            Obstacles = new ObstaclesPool(_obstaclesRoot, _gameplaySettings.Obstacle._prefab);
        }

        public void Dispose() {
            if (Coins != null) {
                Coins.Dispose();
                Coins = null;
            }

            if (Obstacles != null) {
                Obstacles.Dispose();
                Obstacles = null;
            }

            _gameplaySettings = null;
        }
    }
}
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    public class PrefabGameplaySettingsProvider : MonoBehaviour, IGameplaySettingsProvider {

        [SerializeField]
        private LevelSettings _levelSettings;
        public LevelSettings Level => _levelSettings;

        [SerializeField]
        private CharacterSettings _characterSettings;
        public CharacterSettings Character => _characterSettings;

        [SerializeField]
        private CoinSettings _coinSettings;
        public CoinSettings Coin => _coinSettings;

        [SerializeField]
        private ObstacleSettings _obstacleSettings;
        public ObstacleSettings Obstacle => _obstacleSettings;

        public void Dispose() {
        }
    }
}

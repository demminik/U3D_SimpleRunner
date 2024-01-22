using Runner.Gameplay.Core.Pooling;
using UnityEngine;

namespace Runner.Gameplay.Core.Obstacles {

    public class Obstacle : MonoBehaviour, IPoolable {

        private RunnerGameplayFasade _gameplayFasade;

        private void OnTriggerEnter(Collider other) {
            _gameplayFasade.InteractionProcessor.Obstacle.ProcessInteraction(this, other);
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            _gameplayFasade = gameplayFasade;
        }

        public void Dispose() {
            _gameplayFasade = null;
        }

        public void ProcessPoolCreate() {
        }

        public void ProcessPoolRelease() {
        }

        public void ProcessPoolRetrieve() {
        }
    }
}
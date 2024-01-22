using Runner.Gameplay.Core.Characters;
using Runner.Gameplay.Core.Obstacles;
using Runner.Utils;
using UnityEngine;

namespace Runner.Gameplay.Core.Interactions.Obstacles {
    public class ObstacleInteractionLogic : IObstacleInteractionProcessor {

        public void Dispose() {
        }

        public void ProcessInteraction(Obstacle obstacle, Collider other) {
            TryProcessInteracrionWithCharacter(obstacle, other);
        }

        private bool TryProcessInteracrionWithCharacter(Obstacle obstacle, Collider other) {
            if (other.gameObject.layer != LayersHelper.Character.Value) {
                return false;
            }

            var character = other.gameObject.GetComponent<Character>();
            if (character == null) {
                return false;
            }

            character.Die();

            return true;
        }
    }
}
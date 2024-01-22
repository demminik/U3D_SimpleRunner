using Runner.Gameplay.Core.Interactions.Coins;
using Runner.Gameplay.Core.Interactions.Obstacles;

namespace Runner.Gameplay.Core.Interacions {

    public class CommonInteractionProcessor : IInteractionProcessor {

        public ICoinInteractionProcessor Coin { get; private set; }

        public IObstacleInteractionProcessor Obstacle { get; private set; }

        public CommonInteractionProcessor(ICoinInteractionProcessor coinInteraction, IObstacleInteractionProcessor obstacleInteraction) {
            Coin = coinInteraction;
            Obstacle = obstacleInteraction;
        }

        public void Dispose() {
            if (Coin != null) {
                Coin.Dispose();
                Coin = null;
            }

            if (Obstacle != null) {
                Obstacle.Dispose();
                Obstacle = null;
            }
        }
    }
}
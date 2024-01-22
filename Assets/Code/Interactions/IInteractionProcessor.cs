using Runner.Gameplay.Core.Interactions.Coins;
using Runner.Gameplay.Core.Interactions.Obstacles;
using System;

namespace Runner.Gameplay.Core.Interacions {

    public interface IInteractionProcessor : IDisposable {

        public ICoinInteractionProcessor Coin { get; }
        public IObstacleInteractionProcessor Obstacle { get; }
    }
}
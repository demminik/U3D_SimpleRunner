using Runner.Gameplay.Core.Obstacles;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Interactions.Obstacles {

    public interface IObstacleInteractionProcessor : IDisposable {

        void ProcessInteraction(Obstacle obstacle, Collider other);
    }
}
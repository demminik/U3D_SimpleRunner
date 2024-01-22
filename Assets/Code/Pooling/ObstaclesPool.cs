using Runner.Gameplay.Core.Coins;
using Runner.Gameplay.Core.Obstacles;
using UnityEngine;

namespace Runner.Gameplay.Core.Pooling {

    public class ObstaclesPool : GameObjectsPool<Obstacle> {

        public ObstaclesPool(Transform itemsRoot, Obstacle prefab) : base(itemsRoot, prefab) {
        }
    }
}
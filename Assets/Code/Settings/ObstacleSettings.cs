using Runner.Gameplay.Core.Obstacles;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "ObstacleSettings", menuName = "Settings/ObstacleSettings")]
    public class ObstacleSettings : ScriptableObject {

        // TODO: obstacles implementation is very lazy, need to extend to use different obstacles 

        public Obstacle _prefab;
    }
}
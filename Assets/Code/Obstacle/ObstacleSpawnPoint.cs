using UnityEngine;

namespace Runner.Gameplay.Core.Obstacles {

    public class ObstacleSpawnPoint : MonoBehaviour {

        [field: SerializeField]
        public int TrackIndex { get; private set; }
    }
}
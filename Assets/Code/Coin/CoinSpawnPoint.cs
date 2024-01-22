using UnityEngine;

namespace Runner.Gameplay.Core.Coins {

    public class CoinSpawnPoint : MonoBehaviour {

        [field: SerializeField]
        public int TrackIndex { get; private set; }
    }
}
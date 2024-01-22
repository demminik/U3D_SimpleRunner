using Runner.Gameplay.Core.Tiles;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "TileSet", menuName = "Settings/TileSet")]
    public class TileSet : ScriptableObject {

        [Serializable]
        public struct TileEntry {

            public Tile Prefab;
        }

        // TODO: validate data
        [SerializeField]
        private TileEntry[] _availableTiles;
        public TileEntry[] AvailableTiles => _availableTiles;
    }
}
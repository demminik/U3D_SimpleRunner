using Runner.Gameplay.Core.Coins;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "BuffSettings", menuName = "Settings/BuffSettings")]
    public class BuffSettings : ScriptableObject {

        [Serializable]
        public struct TileContentGenerationSettings {

            public int StartTilesWithNoCoins;
            public int StartTilesWithNoObstacles;

            public int CoinsAppearChance;

            public int SpecialCoinAppearChance;
            public int SpecialCoinAppearCooldown;

            public int ObstacleAppearChance;
            public int ObstacleAppearCooldown;

            public SpecialCoinsSet AvailableSpecialCoins;
        }

        [SerializeField]
        private TileSet _tileSet;
        public TileSet TileSet => _tileSet;

        [SerializeField]
        private int _maxPossibleTilesBehindCharacter = 1;
        public int MaxPossibleTilesBehindCharacter => _maxPossibleTilesBehindCharacter;

        [SerializeField]
        private int _maxPossibleTilesTotal = 10;
        public int MaxPossibleTilesTotal => _maxPossibleTilesTotal;

        [SerializeField]
        private TileContentGenerationSettings _contentGenerationSettings = new TileContentGenerationSettings() {
            StartTilesWithNoCoins = 2,
            StartTilesWithNoObstacles = 4,

            CoinsAppearChance = 75,

            SpecialCoinAppearChance = 5,
            SpecialCoinAppearCooldown = 5,

            ObstacleAppearChance = 50,
            ObstacleAppearCooldown = 0,
        };
        public TileContentGenerationSettings ContentGenerationSettings => _contentGenerationSettings;
    }
}
using Runner.Gameplay.Core.Buffs;
using Runner.Gameplay.Core.Coins;
using Runner.Gameplay.Core.Coins.View;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "CoinSettings", menuName = "Settings/CoinSettings")]
    public class CoinSettings : ScriptableObject {

        [Serializable]
        public struct CoinData {

            public static CoinData Empty => new CoinData() {
                CoinType = ECoinType.None,
                ScoreValue = 0,
            };

            public ECoinType CoinType;
            public int ScoreValue;
            public CharacterBuffSettings CharacterBuffs;
        }

        // TODO: validate data

        [SerializeField]
        private Coin _basePrefab;
        public Coin BasePrefab => _basePrefab;

        [SerializeField]
        private CoinSkin[] _skins = new CoinSkin[0];
        public CoinSkin[] Skins => _skins;

        [SerializeField]
        private CoinData[] _data = new CoinData[0];
        public CoinData[] Data => _data;

        // TODO: move outside of settings
        public CoinData GetData(ECoinType coinType) {
            for (int i = 0; i < _data.Length; i++) {
                if (_data[i].CoinType == coinType) {
                    return _data[i];
                }
            }
            return CoinData.Empty;
        }
    }
}
using Runner.Gameplay.Core.Coins;
using Runner.Gameplay.Core.Coins.View;
using Runner.Gameplay.Core.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Gameplay.Core.Pooling {

    public class CoinsPool : GameObjectsPool<Coin> {

        private Dictionary<ECoinType, GameObjectsPool<CoinSkin>> _skinPools = new Dictionary<ECoinType, GameObjectsPool<CoinSkin>>();

        public CoinsPool(Transform itemsRoot, CoinSettings settings) : base(itemsRoot, settings.BasePrefab) {
            foreach (var item in settings.Skins) {
                if (!_skinPools.TryGetValue(item.CoinType, out var suitablePool)) {
                    _skinPools[item.CoinType] = new GameObjectsPool<CoinSkin>(itemsRoot, item); ;
                } else {
                    UnityEngine.Debug.LogError($"Failed to create coins pool for {item.CoinType}: already created");
                }
            }
        }

        public GameObjectsPool<CoinSkin> GetSkinsPool(ECoinType type) {
            if (_skinPools.TryGetValue(type, out var suitablePool)) {
                return suitablePool;
            }

            UnityEngine.Debug.LogError($"Failed to retrieve skins pool for {type} coin" +
                $"\n {StackTraceUtility.ExtractStackTrace()}");
            return null;
        }
    }
}
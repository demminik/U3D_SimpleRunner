using Runner.Gameplay.Core.Coins;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "SpecialCoinsSet", menuName = "Settings/SpecialCoinsSet")]
    public class SpecialCoinsSet : ScriptableObject {

        [Serializable]
        public struct Entry {

            public ECoinType Type;
        }

        // TODO: validate data
        [SerializeField]
        private Entry[] _availableCoins;
        public Entry[] AvailableCoins => _availableCoins;
    }
}
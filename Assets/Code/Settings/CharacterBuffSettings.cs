using Runner.Gameplay.Core.Buffs;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "CharacterBuffSettings", menuName = "Settings/CharacterBuffSettings")]
    public class CharacterBuffSettings : ScriptableObject  {

        [Serializable]
        public struct CharacterBuffData : ICharacterBuff {

            [SerializeField]
            private ECharacterBuffType _type;
            [SerializeField]
            private float _duration;
            [SerializeField]
            private float _floatValue;
            [SerializeField]
            private float _multiplierValue;

            public ECharacterBuffType Type => _type;
            public float Duration => _duration;
            public float FloatValue => _floatValue;
            public float MultiplierValue => _multiplierValue;
        }

        [SerializeField]
        private CharacterBuffData[] _data = new CharacterBuffData[0];
        public CharacterBuffData[] Data => _data;
    }
}
using Runner.Gameplay.Core.Characters;
using Runner.Gameplay.Core.Characters.View;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Settings {

    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Settings/CharacterSettings")]
    public class CharacterSettings : ScriptableObject {

        [Serializable]
        public struct SkinData {

            public CharacterSkin Prefab;
        }

        [Serializable]
        public struct CharacterParameters : ICharacterParametersProvider {

            [field:SerializeField]
            public float MaxSpeed { get; private set; }

            [field: SerializeField]
            public float AcceleractionSpeed { get; private set; }

            [field: SerializeField]
            public float FloatingHeight { get; private set; }
        }

        [field: SerializeField]
        public Character CharacterPrefab { get; private set; }

        [field: SerializeField]
        public SkinData[] AvailableSkins { get; private set; }

        [SerializeField]
        private CharacterParameters _characterParameters;
        public ICharacterParametersProvider CharacterParametersProvider => _characterParameters;
    }
}
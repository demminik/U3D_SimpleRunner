using Runner.Gameplay.Core.Skins;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters.View {

    public class CharacterPresentation : MonoBehaviour, IDisposable {

        [SerializeField]
        private Transform _skinRoot;

        private SkinLogic<CharacterSkin> _skinLogic;
        public SkinLogic<CharacterSkin> SkinLogic => _skinLogic;

        public void Awake() {
            _skinLogic = new SkinLogic<CharacterSkin>(_skinRoot);
        }

        private void OnDestroy() {
            Dispose();
        }

        public void Dispose() {
            if(_skinLogic != null) {
                _skinLogic.ClearCurrentSkin();
                _skinLogic = null;
            }
        }
    }
}
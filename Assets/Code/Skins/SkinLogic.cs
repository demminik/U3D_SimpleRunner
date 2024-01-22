using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Skins {

    public class SkinLogic<T> : IDisposable where T: Component {

        private Transform _skinRoot;

        public bool HasSkin { get; private set; }

        private T _currentSkin;
        public T CurrentSkin {
            get => _currentSkin;
            set {
                _currentSkin = value;
                HasSkin = _currentSkin != null; ;
            }
        }

        public SkinLogic(Transform skinRoot) {
            _skinRoot = skinRoot;
        }

        public void Dispose() {
            _skinRoot = null;
        }

        public void ApplySkinFromPrefab(T skinPrefab) {
            if (skinPrefab == null) {
                return;
            }

            var skinInstance = GameObject.Instantiate(skinPrefab);
            ApplySkin(skinInstance);
        }

        public void ApplySkin(T skinInstance) {
            ClearCurrentSkin();

            CurrentSkin = skinInstance;
            if (HasSkin) {
                CurrentSkin.transform.parent = _skinRoot;
                CurrentSkin.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                CurrentSkin.transform.localScale = Vector3.one;
            }
        }

        public void ClearCurrentSkin() {
            for (int i = 0; i < _skinRoot.childCount; i++) {
                GameObject.Destroy(_skinRoot.GetChild(i).gameObject);
            }
            CurrentSkin = null;
        }
    }
}
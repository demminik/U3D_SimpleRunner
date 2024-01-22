using Runner.Gameplay.Core.Pooling;
using System;
using System.Collections;
using UnityEngine;

namespace Runner.Gameplay.Core.Coins.View {

    public class CoinSkin : MonoBehaviour, IPoolable {

        [SerializeField]
        private ECoinType _coinType;
        public ECoinType CoinType => _coinType;

        [SerializeField]
        private Renderer[] _renderers = new Renderer[0];

        [SerializeField]
        private ParticleSystem _pickupEffect;

        private WaitForSeconds _wfs;

        private void OnDestroy() {
            Dispose();
        }

        public void Dispose() {
            StopAllCoroutines();
        }

        public void Show() {
            StopAllCoroutines();
            _pickupEffect?.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
            SetRendererEnabled(true);
        }

        public virtual void Pickup(Action onFinish) {
            SetRendererEnabled(false);
            StartCoroutine(PickupEffectCoroutine(onFinish));
        }

        protected void SetRendererEnabled(bool isEnabled) {
            foreach (Renderer renderer in _renderers) {
                renderer.enabled = isEnabled;
            }
        }

        protected IEnumerator PickupEffectCoroutine(Action onFinish) {
            if (_pickupEffect != null) {
                if (_wfs == null) {
                    _wfs = new WaitForSeconds(_pickupEffect.main.duration);
                }

                _pickupEffect.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _pickupEffect.Play(withChildren: true);

                yield return _wfs;
            }

            onFinish?.Invoke();
        }

        public void ProcessPoolCreate() {
        }

        public void ProcessPoolRetrieve() {
        }

        public void ProcessPoolRelease() {
            SetRendererEnabled(false);
        }
    }
}
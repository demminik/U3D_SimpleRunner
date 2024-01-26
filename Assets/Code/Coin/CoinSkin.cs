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

        [SerializeField]
        private CoinSkinAnimationLogic _coinSkinAnimationLogic;

        private RunnerGameplayFasade _gameplayFasade;
        private WaitForSeconds _wfs;

        private bool _isInitialized= false;

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            if(_isInitialized) {
                return;
            }

            _gameplayFasade = gameplayFasade;

            if(_coinSkinAnimationLogic != null) {
                _coinSkinAnimationLogic.Initialize(_gameplayFasade);
            }
        }

        public void Dispose() {
            _gameplayFasade = null;
            StopAllCoroutines();
        }

        public void Show() {
            StopAllCoroutines();
            _pickupEffect?.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
            SetActive(true);
        }

        public virtual void Pickup(Action onFinish) {
            SetActive(false);
            StartCoroutine(PickupEffectCoroutine(onFinish));
        }

        private void SetActive(bool isActive) {
            SetRendererEnabled(isActive);
            SetAnimationEnabled(isActive);
        }

        protected void SetRendererEnabled(bool isEnabled) {
            foreach (Renderer renderer in _renderers) {
                renderer.enabled = isEnabled;
            }
        }

        protected void SetAnimationEnabled(bool isEnabled) {
            if (_coinSkinAnimationLogic != null) {
                if(isEnabled) {
                    _coinSkinAnimationLogic.Enable();
                } else {
                    _coinSkinAnimationLogic.Disable();
                }
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
            SetActive(false);
        }
    }
}
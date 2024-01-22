using Runner.Gameplay.Core.Skins;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Coins.View {

    public class CoinPresentation : MonoBehaviour, IDisposable {

        [SerializeField]
        private Transform _skinRoot;

        private RunnerGameplayFasade _gameplayFasade;
        private SkinLogic<CoinSkin> _skinLogic;

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            _gameplayFasade = gameplayFasade;
            _skinLogic = new SkinLogic<CoinSkin>(_skinRoot);
        }

        public void Dispose() {
            _gameplayFasade = null;

            if(_skinLogic != null) {
                _skinLogic.ClearCurrentSkin();
                _skinLogic.Dispose();
                _skinLogic = null;
            }
        }

        public void LoadSkin(ECoinType coinType) {
            var skinsPool = _gameplayFasade.Pooling.Coins.GetSkinsPool(coinType);
            if(skinsPool != null) {
                var skinFromPool = skinsPool.GetItem();
                _skinLogic.ApplySkin(skinFromPool);
            }
        }

        public void ClearCurrentSkin() {
            if (_skinLogic.HasSkin) {
                _gameplayFasade.Pooling.Coins.GetSkinsPool(_skinLogic.CurrentSkin.CoinType).ReleaseItem(_skinLogic.CurrentSkin);
            }

            _skinLogic.ClearCurrentSkin();
        }

        public void Show() {
            if (_skinLogic.HasSkin) {
                _skinLogic.CurrentSkin.Show();
            }
        }

        public void Pickup(Action onFinish) {
            if(!_skinLogic.HasSkin) {
                onFinish?.Invoke();
            }
            _skinLogic.CurrentSkin.Pickup(onFinish);
        }
    }
}
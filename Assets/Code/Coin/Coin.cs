using Runner.Gameplay.Core.Coins.View;
using Runner.Gameplay.Core.Pooling;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Coins {

    public class Coin : MonoBehaviour, IPoolable, IDisposable {

        [SerializeField]
        private CoinPresentation _presentation;

        [SerializeField]
        private CoinData _data;
        public CoinData Data => _data;

        private RunnerGameplayFasade _gameplayFasade;

        private void OnTriggerEnter(Collider other) {
            _gameplayFasade.InteractionProcessor.Coin.ProcessInteraction(this, other);
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade, CoinData data) {
            _gameplayFasade = gameplayFasade;
            _data = data;

            _presentation.Initialize(gameplayFasade);
            _presentation.LoadSkin(data.Type);

            Activate();
        }

        public void Dispose() {
            _gameplayFasade = null;
            _presentation.Dispose();
        }

        public void ProcessPoolCreate() {
        }

        public void ProcessPoolRelease() {
            _presentation.ClearCurrentSkin();
        }

        public void ProcessPoolRetrieve() {
            Activate();
        }

        private void Activate() {
            _presentation.Show();
        }

        public void Pickup() {
            _presentation.Pickup(Release);
        }

        private void Release() {
            _gameplayFasade.Pooling.Coins.ReleaseItem(this);
        }
    }
}
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Timers {

    public class TickProviderDefault : MonoBehaviour, ITickProvider, IDisposable {

        public event Action<float> OnTick;

        private float _timeScale = 1f;
        private float TimeScale {
            get => _timeScale;
            set => _timeScale = value;
        }

        private void Update() {
            OnTick?.Invoke(Time.deltaTime * TimeScale);
        }

        private void OnDestroy() {
            Dispose();
        }

        public void Dispose() {
            OnTick = null;
        }
    }
}
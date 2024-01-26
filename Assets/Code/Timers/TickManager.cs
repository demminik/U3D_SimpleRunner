using System;

namespace Runner.Gameplay.Core.Timers {

    public class TickManager : ITickProvider {

        private ITickProvider _tickProvider;

        public event Action<float> OnTick;

        public TickManager(ITickProvider tickProvider) {
            _tickProvider = tickProvider;

            if(_tickProvider != null) {
                _tickProvider.OnTick += Tick;
            }
        }

        public void Dispose() {
            if (_tickProvider != null) {
                _tickProvider.OnTick -= Tick;
                _tickProvider = null;
            }

            OnTick = null;
        }

        private void Tick(float deltaTime) {
            OnTick?.Invoke(deltaTime);
        }
    }
}
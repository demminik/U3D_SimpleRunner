using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Timers {

    public class GameTime : IDisposable {

        private float _startTime = 0f;

        public float CurrentTime => Time.time - _startTime;

        public ITickProvider GeneralTickManager { get; private set; }

        private bool _isStarted = false;

        public GameTime(ITickProvider generalTickManager) {
            GeneralTickManager = generalTickManager;
        }

        public void StartExecution() {
            if(!_isStarted) {
                _isStarted = true;
                _startTime = Time.time;
            }
        }

        public void Dispose() {
            if (GeneralTickManager != null) {
                GeneralTickManager.Dispose();
                GeneralTickManager = null;
            }
        }
    }
}
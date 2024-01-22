using UnityEngine;

namespace Runner.Gameplay.Core.Timers {

    public class GameTime {

        private float _startTime = 0f;

        public float CurrentTime => Time.time - _startTime;

        public GameTime() {
            _startTime = Time.time;
        }
    }
}
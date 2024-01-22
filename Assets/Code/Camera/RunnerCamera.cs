using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Camera {

    /// <summary>
    /// Very basic camera that follows target
    /// </summary>
    public class RunnerCamera : MonoBehaviour {

        [Serializable]
        private struct CameraSettings {

            public Vector3 Offset;
            public Vector3 Rotation;
        }

        [SerializeField]
        private CameraSettings _settings;

        private Transform _target;
        private bool _hasTarget = false;

        public void SetTarget(Transform target) {
            _target = target;
            _hasTarget = target != null;
        }

        public void Tick(float deltaTime) {
            if (!_hasTarget) {
                return;
            }

            transform.position = _target.position + _settings.Offset;
            transform.rotation = Quaternion.Euler(_settings.Rotation);
        }
    }
}
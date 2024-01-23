using Runner.Gameplay.Core.Movement;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters {

    public class CharacterAnimationLogic : MonoBehaviour, IDisposable {

        private IRunningObject _runningObject;

        private Animator _animator;
        public Animator Animator {
            get => _animator;
            set {
                _animator = value;
            }
        }

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(IRunningObject runningObject) {
            _runningObject = runningObject;
        }

        public void Dispose() {
            _runningObject  = null;
            _animator = null;
        }

        public void Tick(float deltaTime) {
            UpdateAnimationParams();
        }

        private void UpdateAnimationParams() {
            if(Animator != null) {
                // TODO: cache parameter names
                Animator.SetFloat("MovementSpeed", _runningObject.MaxSpeed);
                Animator.SetBool("InAir", _runningObject.IsInAir);
            }
        }
    }
}
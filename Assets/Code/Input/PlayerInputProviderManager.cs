using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Input {

    /// <summary>
    /// Accumulates all the input sources in one place
    /// </summary>
    public class PlayerInputProviderManager : MonoBehaviour, IPlayerInputProvider {

        [SerializeField]
        private bool _allowKeyboard = true;

        [SerializeField]
        private bool _allowTouches = true;

        public event Action OnMoveRight;
        public event Action OnMoveLeft;
        public event Action OnJump;

        private KeyboardPlayerInputProvider _keyboardInputProvider;
        private TouchPlayerInputProvider _touchInputProvider;

        private void OnDestroy() {
            Dispose();
        }

        public void StartExecution() {
            if (_allowKeyboard) {
                _keyboardInputProvider = gameObject.AddComponent<KeyboardPlayerInputProvider>();
                SubscribeToProvider(_keyboardInputProvider);
            }

            if (_allowTouches) {
                _touchInputProvider = gameObject.AddComponent<TouchPlayerInputProvider>();
                SubscribeToProvider(_touchInputProvider);
            }
        }

        public void Dispose() {
            OnMoveRight = null;
            OnMoveLeft = null;
            OnJump = null;

            if (_keyboardInputProvider != null) {
                UnsubscribeFromProvider(_keyboardInputProvider);
                GameObject.Destroy(_keyboardInputProvider);
                _keyboardInputProvider = null;
            }

            if (_touchInputProvider != null) {
                UnsubscribeFromProvider(_touchInputProvider);
                GameObject.Destroy(_touchInputProvider);
                _touchInputProvider = null;
            }
        }

        private void SubscribeToProvider(IPlayerInputProvider provider) {
            UnsubscribeFromProvider(provider);

            provider.OnMoveLeft += ProcessMoveLeft;
            provider.OnMoveRight += ProcessMoveRight;
            provider.OnJump += ProcessJump;
        }

        private void UnsubscribeFromProvider(IPlayerInputProvider provider) {
            provider.OnMoveLeft -= ProcessMoveLeft;
            provider.OnMoveRight -= ProcessMoveRight;
            provider.OnJump -= ProcessJump;
        }

        private void ProcessMoveLeft() {
            OnMoveLeft?.Invoke();
        }

        private void ProcessMoveRight() {
            OnMoveRight?.Invoke();
        }

        private void ProcessJump() {
            OnJump?.Invoke();
        }
    }
}
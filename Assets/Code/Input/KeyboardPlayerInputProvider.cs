using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Input {

    public class KeyboardPlayerInputProvider : MonoBehaviour, IPlayerInputProvider {

        public event Action OnMoveRight;
        public event Action OnMoveLeft;
        public event Action OnJump;

        public void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)) {
                OnMoveRight?.Invoke();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
                OnMoveLeft?.Invoke();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) {
                OnJump?.Invoke();
            }
        }

        public void Dispose() {
            OnMoveRight = null;
            OnMoveLeft = null;
            OnJump = null;
        }
    }
}
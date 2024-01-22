using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Input {

    /// <summary>
    /// Very basic swipe detector, found somewhere at the internet
    /// </summary>
    public class TouchPlayerInputProvider : MonoBehaviour, IPlayerInputProvider {

        public event Action OnMoveRight;
        public event Action OnMoveLeft;
        public event Action OnJump;

        private Vector2 _fingerDownPos;
        private Vector2 _fingerUpPos;

        private bool _detectSwipeAfterRelease = true;

        private float SWIPE_THRESHOLD = 20f;

        private void Update() {

            foreach (Touch touch in UnityEngine.Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    _fingerUpPos = touch.position;
                    _fingerDownPos = touch.position;
                }

                //Detects Swipe while finger is still moving on screen
                if (touch.phase == TouchPhase.Moved) {
                    if (!_detectSwipeAfterRelease) {
                        _fingerDownPos = touch.position;
                        DetectSwipe();
                    }
                }

                //Detects swipe after finger is released from screen
                if (touch.phase == TouchPhase.Ended) {
                    _fingerDownPos = touch.position;
                    DetectSwipe();
                }
            }
        }

        public void Dispose() {
            OnMoveRight = null;
            OnMoveLeft = null;
            OnJump = null;
        }

        private void DetectSwipe() {

            if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue()) {
                //Debug.Log("Vertical Swipe Detected!");
                if (_fingerDownPos.y - _fingerUpPos.y > 0) {
                    OnSwipeUp();
                } else if (_fingerDownPos.y - _fingerUpPos.y < 0) {
                    OnSwipeDown();
                }
                _fingerUpPos = _fingerDownPos;

            } else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue()) {
                //Debug.Log("Horizontal Swipe Detected!");
                if (_fingerDownPos.x - _fingerUpPos.x > 0) {
                    OnSwipeRight();
                } else if (_fingerDownPos.x - _fingerUpPos.x < 0) {
                    OnSwipeLeft();
                }
                _fingerUpPos = _fingerDownPos;

            } else {
                //Debug.Log("No Swipe Detected!");
            }
        }

        private  float VerticalMoveValue() {
            return Mathf.Abs(_fingerDownPos.y - _fingerUpPos.y);
        }

        private  float HorizontalMoveValue() {
            return Mathf.Abs(_fingerDownPos.x - _fingerUpPos.x);
        }

        private void OnSwipeUp() {
            OnJump?.Invoke();
        }

        private void OnSwipeDown() {
        }

        private void OnSwipeLeft() {
            OnMoveLeft?.Invoke();
        }

        private void OnSwipeRight() {
            OnMoveRight?.Invoke();
        }
    }
}
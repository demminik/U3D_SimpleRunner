using Runner.Gameplay.Core.Buffs;
using Runner.Gameplay.Core.Movement;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters {

    /// <summary>
    /// Running object implementation for character
    /// </summary>
    public class CharacterMovementLogic : MonoBehaviour, IDisposable, IRunningObject {

        public bool IsRunning { get; private set; } = false;
        public float MaxSpeed { get; private set; }
        public float AccelerationSpeed { get; private set; }
        public float FloatingHeight { get; private set; }

        public Vector3 CurrentPosition => transform.position;
        public Quaternion CurrentRotation => transform.rotation;
        public bool IsInAir { get; private set; }

        private ICharacterParametersProvider _currentParametersProvider;
        private ICharacterBuffTargetReader _bufflogic;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(ICharacterParametersProvider parametersProvider, ICharacterBuffTargetReader buffLogic) {
            _currentParametersProvider = parametersProvider;

            _bufflogic = buffLogic;
            SubscribeTuBuffs();
            RecalculateParams();
        }

        public void Dispose() {
            _currentParametersProvider = null;

            UnsubscribeFromBuffs();
            _bufflogic = null;

            IsInAir = false;
        }

        public void Tick(float deltaTime) {
            transform.rotation = _targetRotation;

            // TODO: this is lazy smooth movement, need to write correct algorithm for side and vertical movement
            const float maxSpeedLerpMultiplier = 1.5f;
            var targetPosition = _targetPosition + transform.up * FloatingHeight;

            var targetPositionVector = targetPosition - transform.position;
            var newPosition = transform.position + (targetPositionVector.normalized * _currentParametersProvider.MaxSpeed * maxSpeedLerpMultiplier * deltaTime);
            var newPositionVector = newPosition - transform.position;

            // limit movement for not to surpass target mosition
            if(newPositionVector.sqrMagnitude > targetPositionVector.sqrMagnitude) {
                newPosition = targetPosition;
            }
            transform.position = newPosition;
        }

        public void SetStartTransform(Vector3 position, Quaternion rotation) {
            _targetPosition = position;
            _targetRotation = rotation;

            transform.position = position;
            transform.rotation = rotation;
        }

        public void Move(Vector3 targetPosition) {
            _targetPosition = targetPosition;
        }

        public void Rotate(Quaternion targetRotation) {
            _targetRotation = targetRotation;
        }

        public void StartRunning() {
            IsRunning = true;
        }

        public void StopRunning() {
            IsRunning = false;
        }

        private void SubscribeTuBuffs() {
            UnsubscribeFromBuffs();

            if (_bufflogic != null) {
                _bufflogic.OnBuffApplied += ProcessBuffApplied;
                _bufflogic.OnBuffEnded += ProcessBuffEnded;
            }
        }

        private void UnsubscribeFromBuffs() {
            if (_bufflogic != null) {
                _bufflogic.OnBuffApplied -= ProcessBuffApplied;
                _bufflogic.OnBuffEnded -= ProcessBuffEnded;
            }
        }

        private void ProcessBuffApplied(ECharacterBuffType buffType) {
            // TODO: get rid of switch, add processors
            switch (buffType) {
                case ECharacterBuffType.MovementSpeed:
                case ECharacterBuffType.FloatingHeight:
                    RecalculateParams();
                    break;
            }
        }

        private void ProcessBuffEnded(ECharacterBuffType buffType) {
            switch (buffType) {
                case ECharacterBuffType.MovementSpeed:
                case ECharacterBuffType.FloatingHeight:
                    RecalculateParams();
                    break;
            }
        }

        private void ApplyDefaultParams() {
            if (_currentParametersProvider != null) {
                MaxSpeed = _currentParametersProvider.MaxSpeed;
                AccelerationSpeed = _currentParametersProvider.AcceleractionSpeed;
                FloatingHeight = _currentParametersProvider.FloatingHeight;
            } else {
                MaxSpeed = 1f;
                AccelerationSpeed = 1f;
                FloatingHeight = 0f;
            }
        }

        private void RecalculateParams() {
            ApplyDefaultParams();

            if (_bufflogic != null) {
                ApplyMovementSpeedBuff();
                ApplyFloatingHeightBuff();
            }
        }

        private void ApplyMovementSpeedBuff() {
            var movementSpeedBuff = _bufflogic.GetActiveBuff(ECharacterBuffType.MovementSpeed);
            if (movementSpeedBuff != null) {
                MaxSpeed += movementSpeedBuff.FloatValue;
                MaxSpeed *= movementSpeedBuff.MultiplierValue;
            }
        }

        private void ApplyFloatingHeightBuff() {
            var defaultFloatingHeight = FloatingHeight;

            var floatingHeightBuff = _bufflogic.GetActiveBuff(ECharacterBuffType.FloatingHeight);
            if (floatingHeightBuff != null) {
                FloatingHeight += floatingHeightBuff.FloatValue;
                FloatingHeight *= floatingHeightBuff.MultiplierValue;
            }

            // can't be below default height
            FloatingHeight = Mathf.Max(FloatingHeight, defaultFloatingHeight);
            IsInAir = FloatingHeight > defaultFloatingHeight + Mathf.Epsilon;
        }
    }
}
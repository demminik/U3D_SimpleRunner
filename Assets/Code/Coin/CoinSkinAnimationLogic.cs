using Runner.Gameplay.Core.Timers;
using UnityEngine;

namespace Runner.Gameplay.Core.Coins.View {

    /// <summary>
    /// Basic animation logic for coins
    /// </summary>
    public class CoinSkinAnimationLogic : MonoBehaviour, ITickable {

        [SerializeField]
        private Transform _rotationTransform;

        [SerializeField]
        private Transform _verticalMovementTransform;

        [Tooltip("Angles per second")]
        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private AnimationCurve _verticalMovementCurve;

        private RunnerGameplayFasade _gameplayFasade;

        private float _verticalMuvementCurveTime = 1f;

        private void Awake() {
            if(_verticalMovementCurve != null) {
                _verticalMuvementCurveTime = _verticalMovementCurve.keys[_verticalMovementCurve.keys.Length - 1].time;
            }
        }

        private void OnDestroy() {
            Disable();
            _gameplayFasade = null;
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            _gameplayFasade = gameplayFasade;
        }

        public void Tick(float deltaTime) {
            var rotationAngle = _gameplayFasade.GameTime.CurrentTime * _rotationSpeed;
            _rotationTransform.rotation = Quaternion.AngleAxis(rotationAngle, _rotationTransform.up);

            var position = _verticalMovementTransform.localPosition;
            position.y = _verticalMovementCurve.Evaluate(_gameplayFasade.GameTime.CurrentTime % _verticalMuvementCurveTime);
            _verticalMovementTransform.localPosition = position;
        }

        public void Enable() {
            Disable();
            if(_gameplayFasade != null) {
                _gameplayFasade.GameTime.GeneralTickManager.OnTick += Tick;
            }
        }

        public void Disable() {
            if (_gameplayFasade != null && _gameplayFasade.GameTime != null) {
                _gameplayFasade.GameTime.GeneralTickManager.OnTick -= Tick;
            }
        }
    }
}
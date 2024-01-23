using UnityEngine;

namespace Runner.Gameplay.Core.Coins.View {

    /// <summary>
    /// Basic animation logic for coins
    /// </summary>
    public class CoinSkinAnimationLogic : MonoBehaviour {

        [SerializeField]
        private Transform _rotationTransform;

        [SerializeField]
        private Transform _verticalMovementTransform;

        [Tooltip("Angles per second")]
        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private AnimationCurve _verticalMovementCurve;

        // TODO: rotate from single update
        private void Update() {
                var rotationAngle = Time.time * _rotationSpeed;
                _rotationTransform.rotation = Quaternion.AngleAxis(rotationAngle, _rotationTransform.up);

            var position = _verticalMovementTransform.localPosition;
            position.y = _verticalMovementCurve.Evaluate(Time.time % _verticalMovementCurve.keys[_verticalMovementCurve.keys.Length - 1].time);
            _verticalMovementTransform.localPosition = position;
        }
    }
}
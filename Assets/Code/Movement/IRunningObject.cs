using UnityEngine;

namespace Runner.Gameplay.Core.Movement {

    public interface IRunningObject  {

        bool IsRunning { get; }

        float MaxSpeed { get; }
        float AccelerationSpeed { get; }

        public Vector3 CurrentPosition { get; }
        public Quaternion CurrentRotation { get; }
        public bool IsInAir { get; }

        void SetStartTransform(Vector3 position, Quaternion rotation);

        void Move(Vector3 targetPosition);
        void Rotate(Quaternion targetRotation);

        public void MoveAndRotate(Vector3 targetPosition, Quaternion targetRotation) {
            Move(targetPosition);
            Rotate(targetRotation);
        }

        void StartRunning();
        void StopRunning();
    }
}
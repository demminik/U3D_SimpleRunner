using Runner.Gameplay.Core.Levels;
using System;

namespace Runner.Gameplay.Core.Movement {

    public interface IMovementManager : IDisposable {

        void Initialize(RunnerGameplayFasade gameplayFasade, IRunningObject runner, ILevelLogic levelDataProvider);
        void StartExecution();
        void Tick(float deltaTime);
    }
}
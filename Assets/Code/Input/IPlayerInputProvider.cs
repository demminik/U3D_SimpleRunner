using System;

namespace Runner.Gameplay.Core.Input {

    public interface IPlayerInputProvider : IDisposable {

        event Action OnMoveRight;
        event Action OnMoveLeft;
        event Action OnJump;
    }
}
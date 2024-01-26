using System;

namespace Runner.Gameplay.Core.Timers {

    public interface ITickProvider : IDisposable {

        event Action<float> OnTick;
    }
}
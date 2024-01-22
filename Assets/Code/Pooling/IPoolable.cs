using System;

namespace Runner.Gameplay.Core.Pooling {

    public interface IPoolable : IDisposable {

        void ProcessPoolCreate();
        void ProcessPoolRetrieve();
        void ProcessPoolRelease();
    }
}
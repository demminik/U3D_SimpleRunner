using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Gameplay.Core.Pooling {

    public interface IGameplayPoolProvider : IDisposable {

        public CoinsPool Coins { get; }
        public ObstaclesPool Obstacles { get; }
    }
}
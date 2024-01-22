using Runner.Gameplay.Core.Coins;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Interactions.Coins {

    public interface ICoinInteractionProcessor : IDisposable {

        void ProcessInteraction(Coin coin, Collider other);
    }
}
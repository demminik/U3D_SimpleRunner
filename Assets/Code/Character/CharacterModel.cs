using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters {

    /// <summary>
    /// Model to keep character data
    /// </summary>
    public class CharacterModel : MonoBehaviour, IDisposable {

        public bool IsDead { get; private set; } = false;

        private void OnDestroy() {
            Dispose();
        }

        public void Dispose() {
        }

        public void ResetState() {
            IsDead = false;
        }

        private void Die() {
            IsDead = true;
        }
    }
}
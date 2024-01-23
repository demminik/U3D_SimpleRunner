using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters.View {

    public class CharacterSkin : MonoBehaviour, IDisposable {

        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

        public void Dispose() {
        }
    }
}
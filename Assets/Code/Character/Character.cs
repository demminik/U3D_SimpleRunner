using Runner.Gameplay.Core.Buffs;
using Runner.Gameplay.Core.Characters.View;
using Runner.Gameplay.Core.Movement;
using System;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters {

    public class Character : MonoBehaviour {

        [SerializeField]
        private CharacterModel _model;

        [SerializeField]
        private CharacterPresentation _presentation;

        [SerializeField]
        private CharacterMovementLogic _movementLogic;
        public IRunningObject RunningObject => _movementLogic;

        [SerializeField]
        private CharacterBuffLogic _buffLogic;
        public ICharacterBuffTargetWriter BuffTargetWriter => _buffLogic;

        [SerializeField]
        private CharacterAnimationLogic _animationLogic;

        public Action OnDeath;

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            _movementLogic.Initialize(gameplayFasade.Settings.Character.CharacterParametersProvider, _buffLogic);
            _buffLogic.Initialize(gameplayFasade);

            // TODO: apply skin from settings
            _presentation.SkinLogic.ApplySkinFromPrefab(gameplayFasade.Settings.Character.AvailableSkins[0].Prefab);

            _animationLogic.Initialize(RunningObject);
            _animationLogic.Animator = _presentation.SkinLogic.CurrentSkin.Animator;
        }

        public void StartExecution() {
            _movementLogic.StartRunning();
        }

        public void Dispose() {
            _model.Dispose();
            _presentation.Dispose();
            OnDeath = null;
        }

        public void Tick(float deltaTime) {
            _buffLogic.Tick(deltaTime);
            _movementLogic.Tick(deltaTime);
            _animationLogic.Tick(deltaTime);
        }

        public void Die() {
            RunningObject.StopRunning();
            OnDeath?.Invoke();
        }
    }
}
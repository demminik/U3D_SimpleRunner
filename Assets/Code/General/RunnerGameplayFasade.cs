using Runner.Gameplay.Core.Input;
using Runner.Gameplay.Core.Interacions;
using Runner.Gameplay.Core.Pooling;
using Runner.Gameplay.Core.Settings;
using Runner.Gameplay.Core.Timers;
using System;

namespace Runner.Gameplay.Core {

    public class RunnerGameplayFasade : IDisposable {

        public GameTime GameTime;

        public IGameplaySettingsProvider Settings { get; private set; }

        public IPlayerInputProvider PlayerInput { get; private set; }

        public IGameplayPoolProvider Pooling { get; private set; }

        public IInteractionProcessor InteractionProcessor { get; private set; }

        public RunnerGameplayFasade(IGameplaySettingsProvider settingsProvider,
            IPlayerInputProvider playerInputProvider,
            IGameplayPoolProvider poolProvider,
            IInteractionProcessor interactionProcessor) {

            GameTime = new GameTime();

            Settings = settingsProvider;
            PlayerInput = playerInputProvider;
            Pooling = poolProvider;
            InteractionProcessor = interactionProcessor;
        }

        public void Dispose() {
            if (Settings != null) {
                Settings.Dispose();
                Settings = null;
            }

            if (PlayerInput != null) {
                PlayerInput.Dispose();
                PlayerInput = null;
            }

            if (Pooling != null) {
                Pooling.Dispose();
                Pooling = null;
            }

            if(InteractionProcessor != null) {
                InteractionProcessor.Dispose();
                InteractionProcessor = null;
            }

            GameTime = null;
        }
    }
}
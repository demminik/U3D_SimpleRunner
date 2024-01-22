using System;

namespace Runner.Gameplay.Core.Settings {

    public interface IGameplaySettingsProvider : IDisposable {

        // TODO: split each settings to data and presentation

        public LevelSettings Level { get; }

        public CharacterSettings Character { get; }

        public CoinSettings Coin { get; }

        public ObstacleSettings Obstacle { get; }
    }
}
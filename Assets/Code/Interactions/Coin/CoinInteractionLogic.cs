using Runner.Gameplay.Core.Characters;
using Runner.Gameplay.Core.Coins;
using Runner.Gameplay.Core.Player;
using Runner.Gameplay.Core.Settings;
using Runner.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Gameplay.Core.Interactions.Coins {
    public class CoinInteractionLogic : ICoinInteractionProcessor {

        private IGameplaySettingsProvider _settingsProvider;
        private PlayerScore _playerScore;

        private Dictionary<ECoinType, ICoinCharacterInteraction> _coinCharacterInteractions = new Dictionary<ECoinType, ICoinCharacterInteraction>() {
            { ECoinType.Speedup, new CoinCharacterInteractionSpeedup() },
            { ECoinType.Slowdown, new CoinCharacterInteractionSlowdown() },
            { ECoinType.Flight, new CoinCharacterInteractionFlight() },
        };

        public CoinInteractionLogic(IGameplaySettingsProvider settingsProvider, PlayerScore playerScore) {
            _settingsProvider = settingsProvider;
            _playerScore = playerScore;
        }

        public void Dispose() {
            _settingsProvider = null;
            _playerScore = null;
        }

        public void ProcessInteraction(Coin coin, Collider other) {
            TryProcessInteracrionWithCharacter(coin, other);
        }

        private bool TryProcessInteracrionWithCharacter(Coin coin, Collider other) {
            if (other.gameObject.layer != LayersHelper.Character.Value) {
                return false;
            }

            var character = other.gameObject.GetComponent<Character>();
            if (character == null) {
                return false;
            }

            coin.Pickup();

            _playerScore.Add(coin.Data.ScoreValue);
            if(_coinCharacterInteractions.TryGetValue(coin.Data.Type, out var additionalInteractionBehaviour)) {
                additionalInteractionBehaviour.Interact(_settingsProvider, coin, character);
            }

            return true;
        }
    }
}
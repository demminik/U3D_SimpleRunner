using Runner.Gameplay.Core.Characters;
using Runner.Gameplay.Core.Coins;
using Runner.Gameplay.Core.Settings;

namespace Runner.Gameplay.Core.Interactions.Coins {

    public interface ICoinCharacterInteraction {

        void Interact(IGameplaySettingsProvider settingsProvider, Coin coin, Character character) {
            var buffs = settingsProvider.Coin.GetData(coin.Data.Type).CharacterBuffs;
            if (buffs != null && buffs.Data != null) {
                foreach (var buff in buffs.Data) {
                    character.BuffTargetWriter.Apply(buff);
                }
            }
        }
    }
}
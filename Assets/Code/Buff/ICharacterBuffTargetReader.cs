using System;

namespace Runner.Gameplay.Core.Buffs {

    public interface ICharacterBuffTargetReader : IDisposable {

        public ICharacterBuff GetActiveBuff(ECharacterBuffType buffType);

        event Action<ECharacterBuffType> OnBuffApplied;
        event Action<ECharacterBuffType> OnBuffEnded;
    }
}
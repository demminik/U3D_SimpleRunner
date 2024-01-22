using System;

namespace Runner.Gameplay.Core.Buffs {

    public interface ICharacterBuffTargetWriter : IDisposable {

        public void Apply(ICharacterBuff buff);
        public void Remove(ECharacterBuffType buffType);
    }
}
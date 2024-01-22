namespace Runner.Gameplay.Core.Buffs {

    public interface ICharacterBuff {

        public ECharacterBuffType Type { get; }
        public float Duration { get; }

        public float FloatValue { get; }
        public float MultiplierValue { get; }

        // TODO: add action block
    }
}
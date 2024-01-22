namespace Runner.Gameplay.Core.Characters {

    public interface ICharacterParametersProvider {

        public float MaxSpeed { get; }
        public float AcceleractionSpeed { get; }
        public float FloatingHeight { get; }
    }
}
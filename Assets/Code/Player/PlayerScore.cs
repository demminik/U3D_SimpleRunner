namespace Runner.Gameplay.Core.Player {

    public class PlayerScore {

        public int Score { get; private set; }

        public void Reset() {
            Score = 0;
        }

        public void Add(int value) {
            Score += value;
        }
    }
}
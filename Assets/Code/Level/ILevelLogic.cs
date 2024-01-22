using Runner.Gameplay.Core.Tiles;
using UnityEngine;

namespace Runner.Gameplay.Core.Levels {

    public interface ILevelLogic {

        public void UpdateRunnerLocation(int tileIndex);
        Tile GetTile(int tileIndex);
    }
}
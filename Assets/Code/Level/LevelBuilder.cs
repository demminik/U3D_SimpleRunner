using Runner.Gameplay.Core.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Gameplay.Core.Levels {

    public class LevelBuilder : IDisposable, ILevelLogic {

        private LevelGenerator _levelGenerator;

        private RunnerGameplayFasade _gameplayFasade;
        private Transform _tilesRoot;

        private List<Tile> _activeTiles = new List<Tile>();

        private int _nextTileIndex = 0;
        private int _lastKnownRunnerTileIndex = 0;

        public LevelBuilder(RunnerGameplayFasade gameplayFasade, Transform tilesRoot) {
            _gameplayFasade = gameplayFasade;
            _levelGenerator = new LevelGenerator(gameplayFasade);
            _tilesRoot = tilesRoot;
        }

        public void Dispose() {
            _activeTiles.Clear();

            _tilesRoot = null;

            if(_levelGenerator != null ) {
                _levelGenerator.Dispose();
                _levelGenerator = null;
            }
        }

        public void StartExecution() {
            GenerateTilesToTheLimit();
        }

        private void GenerateTilesToTheLimit() {
            _levelGenerator.GenerateNext(_gameplayFasade.Settings.Level.MaxPossibleTilesTotal - _activeTiles.Count, ProcessTileGenerated);
        }

        private void ProcessTileGenerated(Tile tile) {
            if(tile == null) {
                return;
            }

            tile.RuntimeIndex = _nextTileIndex;

            tile.transform.parent = _tilesRoot;
            if (_activeTiles.Count == 0) {
                tile.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            } else {
                var previousTile = _activeTiles[_activeTiles.Count - 1];
                tile.transform.rotation = previousTile.NextBlockBindSlot.transform.rotation;
                tile.transform.position = previousTile.NextBlockBindSlot.transform.position - tile.SelfBindSlot.transform.localPosition;
            }

            _activeTiles.Add(tile);
            _nextTileIndex++;
        }

        private void RemoveTilesBehindRunner() {
            for (int i = _activeTiles.Count - 1; i >= 0; i--) {
                var item = _activeTiles[i];
                if (item.RuntimeIndex < _lastKnownRunnerTileIndex - _gameplayFasade.Settings.Level.MaxPossibleTilesBehindCharacter) {
                    item.RuntimeIndex = -1;
                    _activeTiles.RemoveAt(i);
                    _levelGenerator.ReleaseTile(item);
                }
            }
        }

        public void UpdateRunnerLocation(int tileIndex) {
            if(tileIndex == _lastKnownRunnerTileIndex) {
                return;
            }

            _lastKnownRunnerTileIndex = tileIndex;

            RemoveTilesBehindRunner();
            GenerateTilesToTheLimit();
        }

        public Tile GetTile(int tileIndex) {
            foreach (var item in _activeTiles) {
                if(item.RuntimeIndex == tileIndex) {
                    return item;
                }
            }
            return null;
        }
    }
}
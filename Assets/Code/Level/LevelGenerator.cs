using Runner.Gameplay.Core.Settings;
using Runner.Gameplay.Core.Tiles;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runner.Gameplay.Core.Levels {

    public class LevelGenerator : IDisposable {

        private struct TileContentGenerationInfo {

            public int ObstacleCooldown;
            public int SpecialCoinCooldown;
        }

        private System.Random _randomizer;

        private TileSet _tileSet;

        private Dictionary<Tile, int> _activeTiles = new Dictionary<Tile, int>();
        private Dictionary<int, List<Tile>> _tilesPool = new Dictionary<int, List<Tile>>();

        private RunnerGameplayFasade _gameplayFasade;

        private TileContentGenerationInfo _tileContentGenerationInfo = new TileContentGenerationInfo();
        private int _numTilesGenerated = 0;

        public LevelGenerator(RunnerGameplayFasade gameplayFasade) {
            _gameplayFasade = gameplayFasade;

            _tileSet = gameplayFasade.Settings.Level.TileSet;

            _randomizer = new System.Random();
        }

        public void Dispose() {
            foreach (var kvp in _activeTiles) {
                if (kvp.Key != null) {
                    GameObject.Destroy(kvp.Key.gameObject);
                    kvp.Key.Dispose();
                }
            }
            _activeTiles.Clear();

            foreach (var kvp in _tilesPool) {
                foreach (var item in kvp.Value) {
                    if (item != null) {
                        GameObject.Destroy(item.gameObject);
                        item.Dispose();
                    }
                }

            }
            _tilesPool.Clear();

            _tileSet = null;
            _randomizer = null;
            _gameplayFasade = null;
            _numTilesGenerated = 0;
        }

        public Tile GenerateNext() {
            if (_tileSet.AvailableTiles.Length == 0) {
                UnityEngine.Debug.LogError("[Level] Tile generation failed: no available tiles");
                return null;
            }

            var randomTileIndex = _randomizer.Next(0, _tileSet.AvailableTiles.Length);

            var randomTileEntry = _tileSet.AvailableTiles[randomTileIndex];
            var tileInstance = ProvideTile(randomTileIndex);

            tileInstance.Activate();

            GenerateTileContent(tileInstance);

            _numTilesGenerated++;
            _tileContentGenerationInfo.ObstacleCooldown--;
            _tileContentGenerationInfo.SpecialCoinCooldown--;

            return tileInstance;
        }

        public void GenerateNext(int tilesAmount, Action<Tile> generationResultCallback) {
            if (generationResultCallback == null) {
                UnityEngine.Debug.LogError("[Level] Tile generation failed: invalid callback");
                return;
            }

            for (int i = 0; i < tilesAmount; i++) {
                generationResultCallback(GenerateNext());
            }
        }

        private Tile TryGetTileFromPool(int id) {
            if (!_tilesPool.TryGetValue(id, out var suitableTilesPool)) {
                return null;
            }

            if (suitableTilesPool.Count == 0) {
                return null;
            }

            var result = suitableTilesPool[0];
            suitableTilesPool.RemoveAt(0);
            return result;
        }

        private Tile TryCreateNewTile(int id) {
            if (id < 0 || id >= _tileSet.AvailableTiles.Length) {
                UnityEngine.Debug.LogError($"[Level] failed to create tile." +
                    $"\n id: {id}, AvailableTiles.Length: {_tileSet.AvailableTiles.Length}");
                return null;
            }

            var tilePrefab = _tileSet.AvailableTiles[id].Prefab;
            var newTileInstance = GameObject.Instantiate(tilePrefab).GetComponent<Tile>();
            newTileInstance.Initialize(_gameplayFasade);

            return newTileInstance;
        }

        private Tile ProvideTile(int id) {
            var resultTile = TryGetTileFromPool(id);

            if (resultTile == null) {
                resultTile = TryCreateNewTile(id);
            }

            if (resultTile != null) {
                _activeTiles[resultTile] = id;
            } else {
                UnityEngine.Debug.LogError($"[Level] failed to provide tile." +
                    $"\n id: {id}");
            }

            return resultTile;
        }

        private void PlaceTileInPool(int id, Tile tileInstance) {
            if (!_tilesPool.TryGetValue(id, out var suitablePool)) {
                suitablePool = new List<Tile>();
                _tilesPool[id] = suitablePool;
            }

            suitablePool.Add(tileInstance);
        }

        public void ReleaseTile(Tile tileInstance) {
            if (_activeTiles.TryGetValue(tileInstance, out var id)) {
                tileInstance.Deactivate();
                PlaceTileInPool(id, tileInstance);
            } else {
                UnityEngine.Debug.LogError($"[Level] Tile removel error: failed to find tile in actives list");
                GameObject.Destroy(tileInstance.gameObject);
            }
        }

        private void GenerateTileContent(Tile tile) {
            var contentGenerationSettings = _gameplayFasade.Settings.Level.ContentGenerationSettings;
            
            // first create obstacles
            if (_numTilesGenerated >= _gameplayFasade.Settings.Level.ContentGenerationSettings.StartTilesWithNoObstacles) {
                if (_tileContentGenerationInfo.ObstacleCooldown <= 0 &&
                    IsChanceSuccessful(contentGenerationSettings.ObstacleAppearChance)) {

                    _tileContentGenerationInfo.ObstacleCooldown = contentGenerationSettings.ObstacleAppearCooldown;
                    tile.CreateObstacles(_randomizer.Next(0, tile.Splines.Splines.Count));
                }
            }

            // then coins
            if (_numTilesGenerated >= contentGenerationSettings.StartTilesWithNoCoins) {
                if (IsChanceSuccessful(contentGenerationSettings.CoinsAppearChance)) {

                    var specialCoinType = Coins.ECoinType.Default;

                    if (_tileContentGenerationInfo.SpecialCoinCooldown <= 0
                        && IsChanceSuccessful(contentGenerationSettings.SpecialCoinAppearChance)) {

                        var availableSpecialCoins = _gameplayFasade.Settings.Level.ContentGenerationSettings.AvailableSpecialCoins.AvailableCoins;
                        specialCoinType = availableSpecialCoins.Length > 0 ? availableSpecialCoins[_randomizer.Next(0, availableSpecialCoins.Length)].Type : Coins.ECoinType.Default;
                    }

                    if (tile.CreateCoins(specialCoinType) && specialCoinType != Coins.ECoinType.Default) {
                        _tileContentGenerationInfo.SpecialCoinCooldown = contentGenerationSettings.SpecialCoinAppearCooldown;
                    }
                }
            }
        }

        private bool IsChanceSuccessful(int chance) {
            return _randomizer.Next(0, 101) <= chance;
        }
    }
}
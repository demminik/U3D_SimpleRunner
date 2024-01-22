using Runner.Gameplay.Core.Coins;
using Runner.Gameplay.Core.Obstacles;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;

namespace Runner.Gameplay.Core.Tiles {

    public class Tile : MonoBehaviour, IDisposable {

        [SerializeField]
        private Transform _selfBindSlot;
        public Transform SelfBindSlot => _selfBindSlot;

        [SerializeField]
        private Transform _nextTileBindSlot;
        public Transform NextBlockBindSlot => _nextTileBindSlot;

        [SerializeField]
        private SplineContainer _splines;
        public SplineContainer Splines => _splines;

        //[field:SerializeField]
        public int RuntimeIndex { get; set; } = -1;

        private Dictionary<int, List<CoinSpawnPoint>> _coinSpawnPoints = new Dictionary<int, List<CoinSpawnPoint>>();
        private Dictionary<int, List<ObstacleSpawnPoint>> _obstacleSpawnPoints = new Dictionary<int, List<ObstacleSpawnPoint>>();

        // TODO: add active coins
        // TODO: add active obstacles

        private RunnerGameplayFasade _gameplayFasade;
        private List<int> AvailableLines = new List<int>();

        private System.Random _randomizer;

        private bool _isInitialized = false;

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            if (_isInitialized) {
                return;
            }

            _gameplayFasade = gameplayFasade;
            _randomizer = new System.Random();

            CollectCoinSpawnPoints();
            CollectObstacleSpawnPoints();

            FillAvailableLines();

            _isInitialized = true;
        }

        public void Dispose() {
            _gameplayFasade = null;
        }

        private void CollectCoinSpawnPoints() {
            var spawnPoints = gameObject.GetComponentsInChildren<CoinSpawnPoint>(includeInactive: true);
            foreach (var item in spawnPoints) {
                if (!_coinSpawnPoints.TryGetValue(item.TrackIndex, out var trackSpawnPoints)) {
                    trackSpawnPoints = new List<CoinSpawnPoint>();
                    _coinSpawnPoints[item.TrackIndex] = trackSpawnPoints;
                }

                trackSpawnPoints.Add(item);
            }
        }

        private void CollectObstacleSpawnPoints() {
            var spawnPoints = gameObject.GetComponentsInChildren<ObstacleSpawnPoint>(includeInactive: true);
            foreach (var item in spawnPoints) {
                if (!_obstacleSpawnPoints.TryGetValue(item.TrackIndex, out var trackSpawnPoints)) {
                    trackSpawnPoints = new List<ObstacleSpawnPoint>();
                    _obstacleSpawnPoints[item.TrackIndex] = trackSpawnPoints;
                }

                trackSpawnPoints.Add(item);
            }
        }

        public void Activate() {
            gameObject.SetActive(true);
        }

        public void Deactivate() {
            gameObject.SetActive(false);

            ClearCoins();
            ClearObstacles();

            FillAvailableLines();
        }

        public bool CreateCoins(ECoinType specialCoinType) {
            if(AvailableLines.Count == 0) {
                return false;
            }

            // TODO: generation just for demo purposes, create better one

            // detect available line
            var availableLineIndex = new System.Random().Next(0, AvailableLines.Count);
            var line = AvailableLines[availableLineIndex];
            AvailableLines.RemoveAt(availableLineIndex);

            var coinSpawnPoints = _coinSpawnPoints[line];
            if(coinSpawnPoints.Count == 0) {
                return false;
            }
            var specialCoinIndex = _randomizer.Next(0, coinSpawnPoints.Count);

            // detect required settings
            var defaultCoinType = ECoinType.Default;
            // TODO: use correct settings
            var defaultCoinSettings = _gameplayFasade.Settings.Coin.Data[0];
            var specialCoinSettings = defaultCoinSettings;

            foreach (var item in _gameplayFasade.Settings.Coin.Data) {
                if (item.CoinType == defaultCoinType) {
                    defaultCoinSettings = item;
                }

                if (item.CoinType == specialCoinType) {
                    specialCoinSettings = item;
                }
            }

            // generate
            var generatingCoinIndex = 0;
            foreach (var item in _coinSpawnPoints[line]) {
                var coin = _gameplayFasade.Pooling.Coins.GetItem();

                var coinSettings = generatingCoinIndex != specialCoinIndex ? defaultCoinSettings : specialCoinSettings;
                coin.Initialize(_gameplayFasade, new CoinData() {
                    ScoreValue = coinSettings.ScoreValue,
                    Type = coinSettings.CoinType,
                });
                coin.transform.parent = item.transform;
                coin.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                generatingCoinIndex++;
            }

            return true;
        }

        public void ClearCoins() {
            // TODO: save generated coins for future and rewrite this lazy implementation
            var coins = gameObject.GetComponentsInChildren<Coin>(includeInactive: true);
            foreach (var item in coins) {
                _gameplayFasade.Pooling.Coins.ReleaseItem(item);
            }
        }

        public bool CreateObstacles(int amount) {
            var isSuccess = false;

            amount = Mathf.Min(amount, AvailableLines.Count - 1);   // at least one line should be available for movement

            for (int i = 0; i < amount; i++) {
                var availableLineIndex = _randomizer.Next(0, AvailableLines.Count);
                var line = AvailableLines[availableLineIndex];
                AvailableLines.RemoveAt(availableLineIndex);

                if (!_obstacleSpawnPoints.TryGetValue(line, out var spawnPointsPool) || spawnPointsPool.Count == 0) {
                    //UnityEngine.Debug.LogError($"Obstacle creation failed: can't get spawn point at line {line}", this);
                    continue;
                }

                var spawnPoint = spawnPointsPool[_randomizer.Next(0, spawnPointsPool.Count)];
                var obstacle = _gameplayFasade.Pooling.Obstacles.GetItem();
                obstacle.transform.parent = spawnPoint.transform;
                obstacle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                obstacle.Initialize(_gameplayFasade);

                isSuccess = true;
            }

            return isSuccess;
        }

        public void ClearObstacles() {
            // TODO: save generated obstacles for future and rewrite this lazy implementation
            var obstacles = gameObject.GetComponentsInChildren<Obstacle>(includeInactive: true);
            foreach (var item in obstacles) {
                _gameplayFasade.Pooling.Obstacles.ReleaseItem(item);
            }
        }

        private void FillAvailableLines() {
            AvailableLines.Clear();
            for (int i = 0; i < _splines.Splines.Count; i++) {
                AvailableLines.Add(i);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Gameplay.Core.Pooling {

    public class GameObjectsPool<T> : IDisposable
        where T : UnityEngine.Component, IPoolable {

        private Transform _itemsRoot;
        private T _prefab;

        private List<T> _pool;

        private bool _isSetupValid = false;

        public GameObjectsPool(Transform itemsRoot, T prefab) {
            _isSetupValid = prefab != null;

            if (_isSetupValid) {
                _itemsRoot = itemsRoot;
                _prefab = prefab;

                _pool = new List<T>();
            } else {
                UnityEngine.Debug.LogError($"Failed to initialize {nameof(GameObjectsPool<T>)}: invalid prefab" +
                    $"\n {StackTraceUtility.ExtractStackTrace()}");
            }
        }

        public virtual void Dispose() {
            _itemsRoot = null;
            _prefab = null;

            if (_pool != null) {
                foreach (var item in _pool) {
                    if (item != null) {
                        item.Dispose();
                        GameObject.Destroy(item.gameObject);
                    }
                }
                _pool.Clear();
                _pool = null;
            }
        }

        private T GetItemFromPool() {
            T result = null;
            if (_pool.Count > 0) {
                result = _pool[0];
                _pool.RemoveAt(0);
                result.ProcessPoolRetrieve();
            }
            return result;
        }

        private T CreateItem() {
            if (!_isSetupValid) {
                return null;
            }

            var instance = GameObject.Instantiate(_prefab);
            instance.ProcessPoolCreate();

            return instance;
        }

        public T GetItem() {
            if (!_isSetupValid) {
                return null;
            }

            var item = GetItemFromPool();
            if (item == null) {
                item = CreateItem();
            }

            return item;
        }

        public void ReleaseItem(T item) {
            if (item == null) {
                return;
            }

            // TODO: optimize lookup
            if (_pool.Contains(item)) {
                //UnityEngine.Debug.LogError($"Failed to release pool item: already in pool" +
                //    $"\n {StackTraceUtility.ExtractStackTrace()}", item);
                return;
            }

            _pool.Add(item);
            item.transform.parent = _itemsRoot;
            item.ProcessPoolRelease();
        }
    }
}
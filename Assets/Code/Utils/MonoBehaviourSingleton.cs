using UnityEngine;

namespace Runner.Utils {

    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {

        private static MonoBehaviourSingleton<T> _instance;
        public static MonoBehaviourSingleton<T> Instance {
            get {
                if (_isQuitting) {
                    return null;
                }

                if (!HasInstance) {
                    var go = new GameObject(typeof(MonoBehaviourSingleton<T>).ToString());
                    go.AddComponent<MonoBehaviourSingleton<T>>();
                }

                return _instance;
            }
            private set {
                _instance  = value;
                HasInstance = _instance != null;
            }
        }

        public static bool HasInstance { get; private set; }

        private static bool _isQuitting = false;

        protected virtual void Awake() {
            InitializeInstance();
        }

        protected virtual void OnDestroy() {
            DisposeInstance();
        }

        protected virtual void OnApplicationQuit() {
            _isQuitting = true;
            DisposeInstance();
        }

        private void InitializeInstance() {
            if(Instance != null) {
                UnityEngine.Debug.LogError($"Error when creating singleton of {GetType().ToString()}: instance was already initialized" +
                    $"\n same object: {(Instance == this ? "yes" : "no")}");
                Instance = null;
            }

            if (Instance == null) {
                Instance = this;
            }
        }

        private void DisposeInstance() {
            Instance = null;
        }
    }
}
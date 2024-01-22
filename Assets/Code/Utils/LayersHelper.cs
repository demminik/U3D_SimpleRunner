using UnityEngine;

namespace Runner.Utils {

    public class LayersHelper {

        public struct LayerInfo {

            public int Value;
            public int Mask;
        }

        public static readonly LayerInfo Character;
        public static readonly LayerInfo Terrain;
        public static readonly LayerInfo Coin;
        public static readonly LayerInfo Obstacle;

        static LayersHelper() {
            InitializeLayer("Character", ref Character);
            InitializeLayer("Terrain", ref Terrain);
            InitializeLayer("Coin", ref Coin);
            InitializeLayer("Obstacle", ref Obstacle);
        }

        private static void InitializeLayer(string name, ref LayerInfo layerInfo) {
            layerInfo.Value = LayerMask.NameToLayer(name);
            layerInfo.Mask = 1 << layerInfo.Value;
        }
    }
}
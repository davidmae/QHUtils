using UnityEngine;
using System.Collections;

namespace QHLand
{

    public static class Paths
    {
        public static string QHFolder = "Assets/QHLand/";

        public static string EditorResources = QHFolder + "Editor/Resources/";
        public static string BiomeMaterials = QHFolder + "Resources/Materials/";
        public static string SavedWorlds = QHFolder + "SavedWorlds/";
        public static string NoisePresets = QHFolder + "SavedNoise/Presets/";
        public static string NoiseLayers = QHFolder + "SavedNoise/Layers/";
        public static string GrassGenerators = QHFolder + "SavedDetails/Grass/";

        public static string MyCustomShader = "Nature/Terrain/MyCustomTerrainShader";
        public static string MyCustomShader_NoTransition = "Nature/Terrain/MyCustomTerrainShader_NoTransitions";
    }

}
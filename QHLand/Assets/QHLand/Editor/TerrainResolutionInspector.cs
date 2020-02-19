using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{
    public static class TerrainResolutionInspector
    {
        private static WorldManager manager;
        private static bool show = false;

        public static void SetManager(WorldManager _manager)
        {
            manager = _manager;
        }

        public static void Draw()
        {
            show = EditorGUILayout.Foldout(show, "Terrain Resolution");

            if (show)
            {
                EditorGUILayout.BeginVertical();
                manager.terrainResolution.terrainSize = EditorGUILayout.IntField("Terrain size", manager.terrainResolution.terrainSize);
                manager.terrainResolution.terrainHeight = EditorGUILayout.IntField("Terrain height", manager.terrainResolution.terrainHeight);
                manager.terrainResolution.heightMapResolution = EditorGUILayout.IntField("HeightMapResolution", manager.terrainResolution.heightMapResolution);
                manager.terrainResolution.detailResolution = EditorGUILayout.IntField("DetailResolution", manager.terrainResolution.detailResolution);
                if (manager.terrainResolution.detailResolution < 8) manager.terrainResolution.detailResolution = 8;
                manager.terrainResolution.resolutionPerPatch = EditorGUILayout.IntField("ResolutionPerPatch", manager.terrainResolution.resolutionPerPatch);
                manager.terrainResolution.detailObjectDistance = EditorGUILayout.IntField("DetailObjectDistance", manager.terrainResolution.detailObjectDistance);
                manager.terrainResolution.detailObjectDensity = EditorGUILayout.FloatField("DetailObjectDensity", manager.terrainResolution.detailObjectDensity);
                EditorGUILayout.EndVertical();
            }

            //if (GUILayout.Button("ResetResolution"))
            //    manager.SetTerrainResolution();
        }

    }
}
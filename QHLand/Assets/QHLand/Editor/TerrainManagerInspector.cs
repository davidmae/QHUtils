using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{
    [CustomEditor(typeof(TerrainManager))]
    public class TerrainManagerInspector : AbstractManagerEditor
    {
        MaterialTerrain materialTerrain;
        TerrainManager terrainManager;
        int biomeSelected, lastBiomeSelected;
        Color defaultColor;

        void OnEnable()
        {
            terrainManager = (TerrainManager)target;

            AbstractManagerEditor.manager = terrainManager;

            biomeSelected = terrainManager.biome;
            lastBiomeSelected = biomeSelected;

            if (terrainManager.terrain.materialTemplate == null)
                Debug.LogError("You need generate biomes first");

            materialTerrain = BiomeManager.GetMaterial(terrainManager.biome);

            defaultColor = GUI.color;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (terrainManager.terrain.materialTemplate == null)
                return;

            string[] biomes_str = null;
            int[] biomes_int = null;
            EditorUtils.Enum(ref biomes_str, ref biomes_int, BiomeManager.biomes.Count + 1, BiomeManager.biomes);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (WorldManagerInspector.worldManager.shaderType == eShaderType.Shader_OriginalMode)
            {
                GUILayout.Label("Biome type");

                biomeSelected = EditorGUILayout.IntPopup(biomeSelected, biomes_str, biomes_int);

                if (biomeSelected != lastBiomeSelected)
                {
                    MaterialTerrain newmat = BiomeManager.GetMaterial(biomeSelected);
                    if (newmat != null && materialTerrain != null)
                    {
                        materialTerrain = newmat;
                        terrainManager.biome = biomeSelected;
                        terrainManager.GenerateBiome();
                    }
                }

                lastBiomeSelected = biomeSelected;
            }

            GUI.skin = ResourceLoader.Skin4;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("BiomeSettings", GUILayout.Height(25f)))
            {
                var biomesWindow = BiomeSettingsWindow.CreateInstance<BiomeSettingsWindow>();
                biomesWindow.Init();
                biomesWindow.SetMaterial(materialTerrain);
            }
            if (GUILayout.Button("GrassSettings", GUILayout.Height(25f)))
            {
                var grassWindow = GrassSettingsWindow.CreateInstance<GrassSettingsWindow>();
                grassWindow.Init();
            }
            EditorGUILayout.EndHorizontal();

            if (WorldManagerInspector.worldManager.builded && WorldManagerInspector.worldManager.noised)
            {
                EditorUtils.ButtonPressed("SmoothTerrain", ref terrainManager.smoothTerrainFade, Color.green, defaultColor);
                FadeMenusManager.GetFadeMenu(eFadeMenus.SMOOTH).update(ref terrainManager.smoothTerrainFade);
            }

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
                return;

        }

    }
}
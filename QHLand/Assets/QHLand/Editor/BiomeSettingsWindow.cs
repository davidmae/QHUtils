using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{

    public class BiomeSettingsWindow : EditorWindow
    {
        MaterialTerrain materialTerrain;

        bool dynamicUpdate = false, updateButton = false;
        int biomeSelected = 0, lastBiomeSelected = 1;
        Vector2 scrollpos;
        AbstractManager manager;

        PrettyButton btBiomeSettings;


        public void Init()
        {
            EditorWindow.GetWindow<BiomeSettingsWindow>();
            materialTerrain = BiomeManager.GetMaterial(0);
            manager = AbstractManagerEditor.manager;
            biomeSelected = BiomeManager.biomes.Count + 1;
        }

        public void Init(PrettyButton btBiomeSettings)
        {
            Init();
            this.btBiomeSettings = btBiomeSettings;
            this.btBiomeSettings.executeTicks = false;
        }

        public void SetMaterial(MaterialTerrain material)
        {
            materialTerrain = material;
        }

        void OnGUI()
        {
            DrawWindow();
        }

        void OnDestroy()
        {
            if (this.btBiomeSettings != null)
                this.btBiomeSettings.executeTicks = true;
        }

        void DrawWindow()
        {

            EditorGUILayout.BeginHorizontal();
            {
                dynamicUpdate = GUILayout.Toggle(dynamicUpdate, "Dynamic update");

                if (manager.GetType() == typeof(WorldManager))
                {
                    string[] biomes_str = null; int[] biomes_int = null;
                    EditorUtils.Enum(ref biomes_str, ref biomes_int, BiomeManager.biomes.Count + 1, BiomeManager.biomes);

                    biomes_str[BiomeManager.biomes.Count] = "All";
                    biomes_int[BiomeManager.biomes.Count] = biomes_str.Length;

                    GUILayout.Label("Biome type");

                    biomeSelected = EditorGUILayout.IntPopup(biomeSelected, biomes_str, biomes_int);

                    if (biomeSelected != lastBiomeSelected)
                    {
                        lastBiomeSelected = biomeSelected;
                    }

                    if (GUILayout.Button("+"))
                        NewBiomeWindow.CreateInstance<NewBiomeWindow>().Init();

                }
                else
                    biomeSelected = manager.GetComponent<TerrainManager>().biome;


                MaterialTerrain matTerrain = BiomeManager.GetMaterial(biomeSelected);
                if (matTerrain != null && materialTerrain != null)
                {
                    materialTerrain = matTerrain;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (materialTerrain == null)
                return;

            scrollpos = EditorGUILayout.BeginScrollView(scrollpos, GUILayout.Width(600), GUILayout.Height(500));
            BiomeSettingsInspector.Draw(materialTerrain);
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Update changes"))
                updateButton = true;

            UpdateChanges();
        }

        public void UpdateChanges()
        {
            if (!updateButton && !dynamicUpdate)
                return;

            manager.ChangeMaterialSettings(biomeSelected, materialTerrain, materialTerrain.GetMaterialSettings());

            updateButton = false;
        }

    }
}
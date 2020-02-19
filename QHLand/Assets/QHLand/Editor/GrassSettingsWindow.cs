using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{
    public class GrassSettingsWindow : EditorWindow
    {
        int biomeSelected = 0, layerSelected = 0, generatorSelected = 0;
        bool show = false, show2 = true;
        GUISkin _mySkin;
        AbstractManager manager;
        PrettyButton btGrassSettings;

        public void Init()
        {
            EditorWindow.GetWindow<GrassSettingsWindow>();
            _mySkin = ResourceLoader.Skin1;
            manager = AbstractManagerEditor.manager;
            GrassManager.Load(Paths.GrassGenerators + "GeneratorsDataSaved");
            biomeSelected = BiomeManager.biomes.Count + 1;
        }

        public void Init(PrettyButton btGrassSettings)
        {
            Init();
            this.btGrassSettings = btGrassSettings;
            this.btGrassSettings.executeTicks = false;
        }

        void OnGUI()
        {
            DrawInterface();
        }

        void OnDestroy()
        {
            if (this.btGrassSettings != null)
                this.btGrassSettings.executeTicks = true;
        }

        public void DrawInterface()
        {
            var grassGenerators = GrassManager.GetGenerators();

            GUI.skin = _mySkin;
            GUILayout.Label("Generators");
            GUI.skin = null;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.IntField("Generators: ", grassGenerators.Count);
                if (GUILayout.Button("Add"))
                    grassGenerators.Add(new GrassGenerator());
                if (GUILayout.Button("Delete"))
                {
                    grassGenerators.RemoveAt(generatorSelected);
                    generatorSelected = 0;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (grassGenerators.Count == 0)
                return;

            EditorGUILayout.IntField("Generator selected: ", generatorSelected + 1);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("<"))
                {
                    generatorSelected = (generatorSelected - 1) % grassGenerators.Count;
                    layerSelected = 0;
                }
                if (GUILayout.Button(">"))
                {
                    generatorSelected = (generatorSelected + 1) % grassGenerators.Count;
                    layerSelected = 0;
                }

                if (generatorSelected < 0)
                    generatorSelected = grassGenerators.Count - 1;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.skin = _mySkin;
            GUILayout.Label("Layers");
            GUI.skin = null;

            var selectedGenerator = grassGenerators[generatorSelected];

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.IntField("Layer size: ", selectedGenerator.grassLayers.Count);
                if (GUILayout.Button("Add"))
                    selectedGenerator.grassLayers.Add(new GrassDataLayer());
                if (GUILayout.Button("Duplicate"))
                    selectedGenerator.grassLayers.Add(new GrassDataLayer(selectedGenerator.grassLayers[layerSelected]));
                if (GUILayout.Button("Delete"))
                {
                    selectedGenerator.grassLayers.RemoveAt(layerSelected);
                    layerSelected = 0;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (selectedGenerator.grassLayers.Count == 0)
                return;

            show2 = EditorGUILayout.Foldout(show2, "LayerSettings");

            if (show2)
            {

                EditorGUILayout.IntField("Layer selected: ", layerSelected + 1);

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("<"))
                        layerSelected = (layerSelected - 1) % selectedGenerator.grassLayers.Count;
                    if (GUILayout.Button(">"))
                        layerSelected = (layerSelected + 1) % selectedGenerator.grassLayers.Count;

                    if (layerSelected < 0)
                        layerSelected = selectedGenerator.grassLayers.Count - 1;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                var grassData = selectedGenerator.grassLayers[layerSelected];

                grassData.usePrototypeMesh = EditorGUILayout.Toggle("UsePrototypeMes", grassData.usePrototypeMesh);
                grassData.detailTexture = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), "DetailTexture", grassData.detailTexture, typeof(Texture2D), true) as Texture2D;
                grassData.detailMesh = EditorGUILayout.ObjectField("DetailMesh", grassData.detailMesh, typeof(GameObject), true) as GameObject;
                grassData.detailCountPerDetailPixel = EditorGUILayout.IntField("DetailCountPerDetailPixel", grassData.detailCountPerDetailPixel);
                grassData.minSpawnH = EditorGUILayout.FloatField("MinSpawnH", grassData.minSpawnH);
                grassData.maxSpawnH = EditorGUILayout.FloatField("MaxSpanwH", grassData.maxSpawnH);
                grassData.minSlope = EditorGUILayout.FloatField("MinSlope", grassData.minSlope);
                grassData.maxSlope = EditorGUILayout.FloatField("MaxSlope", grassData.maxSlope);

                grassData.perlinTreshold = EditorGUILayout.Slider("PerlinTreshold", grassData.perlinTreshold, 0f, 1f);
                grassData.inverse = EditorGUILayout.Toggle("Inverse", grassData.inverse);
                grassData.frequency = EditorGUILayout.FloatField("Frequency", grassData.frequency);
                grassData.seed = EditorGUILayout.FloatField("Seed", grassData.seed);
                grassData.disable = EditorGUILayout.Toggle("Disable", grassData.disable);

                show = EditorGUILayout.Foldout(show, "Details");

                if (show)
                {
                    grassData.details.minHeight = EditorGUILayout.FloatField("MinHeight", grassData.details.minHeight);
                    grassData.details.maxHeight = EditorGUILayout.FloatField("MaxHeight", grassData.details.maxHeight);
                    grassData.details.minWidth = EditorGUILayout.FloatField("MinWidth", grassData.details.minWidth);
                    grassData.details.maxWidth = EditorGUILayout.FloatField("MaxWidth", grassData.details.maxWidth);
                    grassData.details.dryColor = EditorGUILayout.ColorField("DryColor", grassData.details.dryColor);
                    grassData.details.healthyColor = EditorGUILayout.ColorField("HealthyColor", grassData.details.healthyColor);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            if (manager.GetType() == typeof(WorldManager))
            {
                EditorGUILayout.BeginHorizontal();
                {
                    string[] biomes_str = null; int[] biomes_int = null;

                    EditorUtils.Enum(ref biomes_str, ref biomes_int, BiomeManager.biomes.Count + 1, BiomeManager.biomes);

                    biomes_str[BiomeManager.biomes.Count] = "All";
                    biomes_int[BiomeManager.biomes.Count] = biomes_str.Length;

                    GUILayout.Label("Apply in biome: ");

                    biomeSelected = EditorGUILayout.IntPopup(biomeSelected, biomes_str, biomes_int);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            else
                biomeSelected = manager.GetComponent<TerrainManager>().biome;

            if (GUILayout.Button("GenerateGrass", GUILayout.Height(50)))
                manager.GenerateGrass(biomeSelected, generatorSelected);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load"))
                GrassManager.Load(Paths.GrassGenerators + "GeneratorsDataSaved");
            if (GUILayout.Button("Save"))
                GrassManager.Save(Paths.GrassGenerators + "GeneratorsDataSaved");
            EditorGUILayout.EndHorizontal();
        }
    }

}
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    [CustomEditor(typeof(WorldManager))]
    public class WorldManagerInspector : AbstractManagerEditor
    {
        public static WorldManager worldManager;
        public static WorldManagerInspector worldManagerInspector;

        public static Color defaultColor;

        GUISkin _mySkin;

        int opt;

        PrettyButton btBuild, btBiomes, btNoise, btWater, btSmooth, btTransitions,
                     btStitching, btBiomeSettings, btGrassSettings, btReset, btSaveLoad;


        void OnEnable()
        {
            worldManagerInspector = this;

            ResourceLoader.LoadResources();
            BiomeManager.FillBiomeList();
            FadeMenusManager.Clear();

            worldManager = (WorldManager)target;

            AbstractManagerEditor.manager = worldManager;

            _mySkin = ResourceLoader.Skin1;
            defaultColor = GUI.color;


            FadeMenusManager.Add(FadeMenus.CreateInstance<NoiseFadeMenu>(), eFadeMenus.NOISE);
            FadeMenusManager.Add(FadeMenus.CreateInstance<WaterFadeMenu>(), eFadeMenus.WATER);
            FadeMenusManager.Add(FadeMenus.CreateInstance<SmoothTerrain>(), eFadeMenus.SMOOTH);
            FadeMenusManager.Add(FadeMenus.CreateInstance<SaveLoadFadeMenu>(), eFadeMenus.SAVELOAD);

            FadeMenusManager.SetMenusSkin(_mySkin);

            btBuild = new PrettyButton("Build", -1);
            btBiomes = new PrettyButton("GenerateBiomes", -1);
            btNoise = new PrettyButton("ConfigureNoise", 1);
            btWater = new PrettyButton("WaterSettings", 1);
            btSmooth = new PrettyButton("SmoothTerrain", 1);
            btTransitions = new PrettyButton("DrawTransitions", 1);
            btStitching = new PrettyButton("StitchingChunks", 1);
            btBiomeSettings = new PrettyButton("BiomeSettings", 1);
            btGrassSettings = new PrettyButton("GrassSettings", 1);
            btReset = new PrettyButton("ResetWorld", 1);
            btSaveLoad = new PrettyButton("Save&Load", 1);


            TerrainResolutionInspector.SetManager(worldManager);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawInterface();
            this.Repaint();
        }

        private void DrawInterface()
        {
            GUI.enabled = true;

            string[] optionTextures_str = null; int[] optionTextures_int = null;
            EditorUtils.Enum(ref optionTextures_str,
                                ref optionTextures_int, 2,
                                new List<string>(){ "5 textures | multiple-biomes | mid quality", 
                                                "6 textures | unique-biome | high quality" });
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Graphics");
            opt = EditorGUILayout.IntPopup(opt, optionTextures_str, optionTextures_int);
            worldManager.shaderType = (eShaderType)opt;
            EditorGUILayout.EndHorizontal();

            worldManager.chunksX = EditorGUILayout.IntField("ChunksX", worldManager.chunksX);
            worldManager.chunksY = EditorGUILayout.IntField("ChunksY", worldManager.chunksY);

            TerrainResolutionInspector.Draw();
            GUI.skin = ResourceLoader.Skin4;

            if (btBuild.PButton(!worldManager.builded, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f))
            {
                worldManager.DestroyChilds();
                worldManager.InitGrid();
                worldManager.builded = true;
                worldManager.texturized = false;
                worldManager.noised = false;
                worldManager.stitched = false;
                worldManager.transitioned = false;
            }

            if (worldManager.builded)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                if (btBiomes.PButton(!worldManager.texturized, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f))
                {
                    worldManager.GenerateBiome();
                    worldManager.texturized = true;
                    btNoise.Repaint(-1);
                }

                EditorUtils.ButtonPressed(btNoise, ref worldManager.noiseMenuFade, EditorUtils.Blue1, defaultColor);
                FadeMenusManager.GetFadeMenu(eFadeMenus.NOISE).update(ref worldManager.noiseMenuFade);

                EditorUtils.ButtonPressed(btWater, ref worldManager.waterMenuFade, EditorUtils.Blue1, defaultColor);
                FadeMenusManager.GetFadeMenu(eFadeMenus.WATER).update(ref worldManager.waterMenuFade);

                EditorGUILayout.Separator();
            }
            else
            {
                btNoise.Repaint(1);
                btWater.Repaint();
            }



            if (worldManager.builded && worldManager.texturized)
            {
                EditorGUILayout.BeginHorizontal();


                if (btBiomeSettings.PButton(worldManager.texturized, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f, GUILayout.Height(25f)))
                {
                    var biomesWindow = BiomeSettingsWindow.CreateInstance<BiomeSettingsWindow>();
                    biomesWindow.Init(btBiomeSettings);
                }

                if (btGrassSettings.PButton(worldManager.texturized, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f, GUILayout.Height(25f)))
                {
                    var grassWindow = GrassSettingsWindow.CreateInstance<GrassSettingsWindow>();
                    grassWindow.Init(btGrassSettings);
                }

                EditorGUILayout.EndHorizontal();

                if (worldManager.shaderType == eShaderType.Shader_OriginalMode && (worldManager.chunksX > 1 || worldManager.chunksY > 1))
                {
                    if (btTransitions.PButton(worldManager.texturized, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f) ||
                        (!Transitioner.Instance.firstTransition && worldManager.transitioned))
                    {
                        worldManager.DrawTransitions();
                        worldManager.transitioned = true;
                    }
                }
            }
            else
            {
                btBiomeSettings.Repaint();
                btGrassSettings.Repaint();
                btTransitions.Repaint();
            }


            if (worldManager.builded && worldManager.noised)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                btNoise.Repaint(1);

                EditorUtils.ButtonPressed(btSmooth, ref worldManager.smoothTerrainFade, EditorUtils.Blue1, defaultColor);
                FadeMenusManager.GetFadeMenu(eFadeMenus.SMOOTH).update(ref worldManager.smoothTerrainFade);

                if (worldManager.chunksX > 1 || worldManager.chunksY > 1)
                {
                    if (btStitching.PButton(!worldManager.stitched, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f))
                    {
                        worldManager.StitchingChunks();
                        worldManager.stitched = true;
                    }
                }
                GUI.color = defaultColor;

            }
            else
            {
                btSmooth.Repaint();
                btStitching.Repaint();
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            if (btReset.PButton(!worldManager.builded, defaultColor, EditorUtils.Blue1, System.DateTime.Now.Millisecond * 0.00001f))
            //if (GUILayout.Button("ResetWorld"))
            {
                worldManager.DestroyChilds();
                worldManager.InitGrid();
                worldManager.builded = false; worldManager.texturized = false;
                worldManager.noised = false; worldManager.stitched = false;
            }
            EditorUtils.ButtonPressed(btSaveLoad, ref worldManager.saveloadMenuFade, EditorUtils.Blue1, defaultColor);

            //if (worldManager.builded) btSaveLoad.Repaint();

            EditorGUILayout.EndHorizontal();
            FadeMenusManager.GetFadeMenu(eFadeMenus.SAVELOAD).update(ref worldManager.saveloadMenuFade);
        }

    }
}
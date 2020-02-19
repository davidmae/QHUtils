using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    public class FadeMenus : ScriptableObject
    {
        protected AbstractManager manager;
        protected GUISkin _mySkin;

        private eFadeMenus id;

        public FadeMenus()
        {
        }

        public virtual void update(ref float value)
        {
            if (value > 0f)
                value += (System.DateTime.Now.Millisecond * 0.0001f);

            if (value > 1f)
                value = 1f;
        }

        public void SetSkin(GUISkin _mySkin)
        {
            this._mySkin = _mySkin;
        }

        public void SetManager(AbstractManager manager)
        {
            this.manager = manager;
        }

        public void SetID(eFadeMenus id)
        {
            this.id = id;
        }

        public eFadeMenus GetID()
        {
            return id;
        }

        public void ResetSkin()
        {
            GUI.skin = ResourceLoader.Skin4;
        }
    }

    public class NoiseFadeMenu : FadeMenus
    {
        NoiseLayer noiseLayer;

        int noise_selected = 0;
        bool dynamicUpdate = false;
        int loctaves = 0;
        float lpersistence = 0f, llacunarity = 0f, lfrequency = 0f, lsize = 0f, lseedx = 0f, lseedy = 0f;
        bool show = true, show2 = false;
        bool addnoise = false, clearnoise = false;
        bool allLayers = false;
        float height;

        System.IO.FileInfo[] presets;
        float[] presetData = new float[6];
        string presetName = "Preset";
        int preset_selected = 0;


        public NoiseFadeMenu() : base() { UpdatePresets(); }

        public override void update(ref float value)
        {
            base.update(ref value);
            Draw(ref value);
            ResetSkin();
        }

        private void Draw(ref float value)
        {
            if (EditorGUILayout.BeginFadeGroup(value))
            {
                GUI.skin = ResourceLoader.Skin3;
                EditorGUILayout.IntField("id", NoiseManager.currLayer + 1);
                GUI.skin = null;

                GUI.skin = ResourceLoader.Skin2;
                for (int i = 0; i < NoiseManager.noiseLayers.Count; ++i)
                {
                    if (GUILayout.Button("NoiseLayer " + (i + 1) + "   ■   " + NoiseManager.noiseLayers[i].noiseData.presetName))
                        NoiseManager.currLayer = i;
                }
                GUI.skin = null;


                GUI.skin = ResourceLoader.Skin3;
                EditorGUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("+"))
                {
                    NoiseManager.noiseLayers.Add(new NoiseLayer());
                    NoiseManager.Next();
                }
                if (GUILayout.Button("-"))
                {
                    NoiseManager.noiseLayers.RemoveAt(NoiseManager.currLayer);
                    NoiseManager.Next();
                }
                if (GUILayout.Button("Save"))
                    NoiseManager.Save(Paths.NoiseLayers + "NoiseLayerDataSaved");

                if (GUILayout.Button("Load"))
                    NoiseManager.Load(Paths.NoiseLayers + "NoiseLayerDataSaved");

                EditorGUILayout.EndHorizontal();
                GUI.skin = null;

                ResetSkin();

                if (NoiseManager.noiseLayers.Count != 0)
                {
                    EditorGUILayout.Separator();

                    noiseLayer = NoiseManager.noiseLayers[NoiseManager.currLayer];

                    if (!dynamicUpdate)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("ApplyLayer"))
                            addnoise = true;

                        if (GUILayout.Button("ClearLayer"))
                            clearnoise = true;
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("ApplyAllLayers"))
                        {
                            addnoise = true;
                            allLayers = true;
                        }

                        if (GUILayout.Button("ClearAllLayers"))
                        {
                            clearnoise = true;
                            allLayers = true;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        height = EditorGUILayout.Slider("Modify base height ", height, -1f, 1f);
                        if (GUILayout.Button("Apply"))
                        {
                            WorldManagerInspector.worldManager.ChangeBaseHeight(height);
                            WorldManagerInspector.worldManager.noised = true;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();


                    noiseLayer.seed = EditorGUILayout.Vector2Field("Seed", noiseLayer.seed);
                    noiseLayer.seedIgnore = EditorGUILayout.Toggle("RandSeed", noiseLayer.seedIgnore);
                    noiseLayer.islandMode = EditorGUILayout.Toggle("IslandMode", noiseLayer.islandMode);

                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();

                    show2 = EditorGUILayout.Foldout(show2, "NoiseData");
                    if (show2)
                    {
                        GUI.skin = null;

                        EditorGUILayout.BeginHorizontal();
                        string[] noise_str = new string[3]; int[] noise_int = new int[3];
                        noise_str[0] = eNoise.FRACTAL.ToString(); noise_int[0] = 0;
                        noise_str[1] = eNoise.FBM.ToString(); noise_int[1] = 1;
                        noise_str[2] = eNoise.BILLOW.ToString(); noise_int[2] = 2;

                        noise_selected = EditorGUILayout.IntPopup((int)(noiseLayer.noiseData.type), noise_str, noise_int);
                        dynamicUpdate = GUILayout.Toggle(dynamicUpdate, "DynamicUpdate");

                        noiseLayer.noiseData.type = (eNoise)(noise_selected);

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Octaves");

                        noiseLayer.noiseData.octaves = EditorGUILayout.IntSlider(noiseLayer.noiseData.octaves, 1, 8);

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Persistence");
                        noiseLayer.noiseData.persistence = EditorGUILayout.Slider(noiseLayer.noiseData.persistence, 0, 2);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Lacunarity");
                        noiseLayer.noiseData.lacunarity = EditorGUILayout.Slider(noiseLayer.noiseData.lacunarity, 1, 15);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Frequency");
                        noiseLayer.noiseData.frequency = EditorGUILayout.Slider(noiseLayer.noiseData.frequency, 0, 100);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Size");
                        noiseLayer.noiseData.size = EditorGUILayout.Slider(noiseLayer.noiseData.size, -50, 50);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Separator();

                        ResetSkin();

                        show = EditorGUILayout.Foldout(show, "Save/Load Presets");
                        if (show)
                            PresetHandler();
                    }

                    if (addnoise || CheckDynamicUpdate())
                    {
                        if (allLayers)
                        {
                            for (int i = 0; i < NoiseManager.noiseLayers.Count; ++i)
                            {
                                manager.GenerateNoise(dynamicUpdate);
                                NoiseManager.Next();
                            }
                            allLayers = false;
                        }
                        else
                            manager.GenerateNoise(dynamicUpdate);

                        WorldManagerInspector.worldManager.noised = true;
                        addnoise = false;

                    }

                    if (clearnoise)
                    {
                        if (allLayers)
                        {
                            foreach (NoiseLayer layer in NoiseManager.noiseLayers)
                            {
                                layer.noiseData.size = -1f * layer.noiseData.size;
                                manager.GenerateNoise(dynamicUpdate);
                                layer.noiseData.size = -1f * layer.noiseData.size;
                                NoiseManager.Next();
                            }
                            allLayers = false;
                        }
                        else
                        {
                            noiseLayer.noiseData.size = -1f * noiseLayer.noiseData.size;
                            manager.GenerateNoise(dynamicUpdate);
                            noiseLayer.noiseData.size = -1f * noiseLayer.noiseData.size;
                        }

                        WorldManagerInspector.worldManager.noised = true;
                        clearnoise = false;
                    }

                    SetLastValues();

                }

                GUI.skin = null;

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();

            }
            EditorGUILayout.EndFadeGroup();
        }

        private bool CheckDynamicUpdate()
        {
            if (dynamicUpdate)
            {
                if (loctaves != noiseLayer.noiseData.octaves || lpersistence != noiseLayer.noiseData.persistence || llacunarity != noiseLayer.noiseData.lacunarity || lfrequency != noiseLayer.noiseData.frequency
                    || lsize != noiseLayer.noiseData.size || lseedx != noiseLayer.seed.x || lseedy != noiseLayer.seed.y)
                    return true;
            }

            return false;
        }

        private void SetLastValues()
        {
            loctaves = noiseLayer.noiseData.octaves;
            lpersistence = noiseLayer.noiseData.persistence;
            lfrequency = noiseLayer.noiseData.frequency;
            llacunarity = noiseLayer.noiseData.lacunarity;
            lsize = noiseLayer.noiseData.size;
            lseedx = noiseLayer.seed.x;
            lseedy = noiseLayer.seed.y;
        }

        private void UpdatePresets()
        {
            presets = ResourceLoader.LoadAllFilesIn(Paths.NoisePresets);

            SaveHandler.Load(Paths.NoisePresets + presets[preset_selected].Name, false);
            presetData = (float[])(SaveHandler.deserializedObject);
            presetName = presets[preset_selected].Name;

            if (manager == null) return;

            noiseLayer.noiseData.type = (eNoise)(presetData[0]);
            noiseLayer.noiseData.presetName = presetName;
            noiseLayer.noiseData.octaves = (int)(presetData[1]);
            noiseLayer.noiseData.persistence = presetData[2];
            noiseLayer.noiseData.lacunarity = presetData[3];
            noiseLayer.noiseData.frequency = presetData[4];
            noiseLayer.noiseData.size = presetData[5];
        }

        private void PresetHandler()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("<<<"))
            {
                PrevPreset();
                GUI.FocusControl("");
            }
            GUI.skin.textField.fontStyle = FontStyle.Bold;
            presetName = EditorGUILayout.TextField("", presetName);
            GUI.skin.textField.fontStyle = FontStyle.Normal;
            if (GUILayout.Button(">>>"))
            {
                NextPreset();
                GUI.FocusControl("");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                presetData[0] = (float)(noiseLayer.noiseData.type);
                presetData[1] = (float)(noiseLayer.noiseData.octaves);
                presetData[2] = noiseLayer.noiseData.persistence;
                presetData[3] = noiseLayer.noiseData.lacunarity;
                presetData[4] = noiseLayer.noiseData.frequency;
                presetData[5] = noiseLayer.noiseData.size;

                SaveHandler.saveObject = presetData;
                SaveHandler.Save(Paths.NoisePresets + presetName, true);

                NextPreset();
                GUI.FocusControl("");

                UpdatePresets();
            }

            if (GUILayout.Button("Delete"))
            {
                FileUtil.DeleteFileOrDirectory(Paths.NoisePresets + presetName);

                NextPreset();
                GUI.FocusControl("");
            }

            EditorGUILayout.EndHorizontal();
        }

        private void NextPreset()
        {
            do
                preset_selected = (++preset_selected) % presets.Length;
            while (presets[preset_selected].Extension.Contains("meta"));

            UpdatePresets();
        }

        private void PrevPreset()
        {
            do
                preset_selected = (preset_selected - 1 < 0 ? presets.Length - 1 : preset_selected - 1);
            while (presets[preset_selected].Extension.Contains("meta"));

            UpdatePresets();
        }

    }

    public class WaterFadeMenu : FadeMenus
    {
        public WaterFadeMenu() : base() { }

        public override void update(ref float value)
        {
            base.update(ref value);

            if (EditorGUILayout.BeginFadeGroup(value))
            {
                ResetSkin();

                manager.waterModel = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), "WaterModel", manager.waterModel, typeof(GameObject), true) as GameObject;
                manager.waterHeight = EditorGUILayout.FloatField("WaterHeight", manager.waterHeight);

                EditorGUILayout.BeginHorizontal();
                manager.showWater = EditorGUILayout.Toggle("ShowWater", manager.showWater);
                if (GUILayout.Button("Update"))
                {
                    manager.DestroyWaterLayer();
                    manager.CreateWaterLayer(0, 0, 0, 0);
                }
                EditorGUILayout.EndHorizontal();

                GUI.skin = null;

                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();
            }
            EditorGUILayout.EndFadeGroup();

            manager.UpdateWater();

            ResetSkin();
        }
    }

    public class SmoothTerrain : FadeMenus
    {
        private int smoothdegree = 0;

        public SmoothTerrain() : base() { }

        public override void update(ref float value)
        {
            base.update(ref value);

            if (EditorGUILayout.BeginFadeGroup(value))
            {
                smoothdegree = EditorGUILayout.IntField("SmoothIterations", smoothdegree);
                if (GUILayout.Button("SmoothAllTerrain"))
                    manager.SmoothTerrain(smoothdegree);
                manager.startCoast = EditorGUILayout.IntField("StartSmoothCoast", manager.startCoast);
                if (GUILayout.Button("Smooth Borders"))
                    manager.SmoothBorders();

                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();
            }
            EditorGUILayout.EndFadeGroup();

            ResetSkin();
        }
    }

    public class SaveLoadFadeMenu : FadeMenus
    {
        private string filename;

        public SaveLoadFadeMenu() : base() { filename = WorldManagerInspector.worldManager.nameScene; }

        public override void update(ref float value)
        {
            base.update(ref value);

            if (EditorGUILayout.BeginFadeGroup(value))
            {

                filename = EditorGUILayout.TextField("Name: ", filename);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save"))
                    manager.Save(Paths.SavedWorlds + filename + ".data");

                if (GUILayout.Button("Load"))
                    manager.Load(Paths.SavedWorlds + filename + ".data");

                EditorGUILayout.EndHorizontal();

                GUI.skin = null;
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                EditorGUILayout.Separator();
            }
            EditorGUILayout.EndFadeGroup();
            ResetSkin();
        }
    }
}
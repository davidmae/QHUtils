using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{

    //[CustomEditor(typeof(BiomeManager))]
    public class TransitionsManagerWindow : EditorWindow
    {
        MaterialTerrain thisMaterial;
        MaterialTerrain.MaterialSettings materialSettings;
        bool showT1Textures = false, showT2Textures = false;
        Texture2D btp0, btp1, btp2, btp3, btp4, btt0, btt1, btt2, btt3;
        GUISkin _mySkin;

        //[MenuItem("Terrain/BiomeManager")]
        public void Init()
        {
            EditorWindow.GetWindow<TransitionsManagerWindow>();
        }

        public void SetMaterial(MaterialTerrain material)
        {
            thisMaterial = material;

            /*btp0 = ResourceLoader.ButtomTexturePosition0;
            btp1 = ResourceLoader.ButtomTexturePosition1;
            btp2 = ResourceLoader.ButtomTexturePosition2;
            btp3 = ResourceLoader.ButtomTexturePosition3;
            btp4 = ResourceLoader.ButtomTexturePosition4;*/

            btt0 = ResourceLoader.ButtomTextureType0;
            btt1 = ResourceLoader.ButtomTextureType1;
            btt2 = ResourceLoader.ButtomTextureType2;
            btt3 = ResourceLoader.ButtomTextureType3;

            _mySkin = ResourceLoader.Skin1;
        }

        void OnGUI()
        {
            materialSettings = thisMaterial.GetTransitionSettingsFromShader();

            GUI.skin = _mySkin;
            GUILayout.Label("Transition settings");
            GUI.skin = null;

            EditorGUILayout.BeginHorizontal();
            {
                materialSettings.transitionLength1 = EditorGUILayout.FloatField("Transition1 Lenght", materialSettings.transitionLength1);
                materialSettings.transitionLength2 = EditorGUILayout.FloatField("Transition2 Lenght", materialSettings.transitionLength2);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel("Transition1 Position");
                        EditorGUILayout.FloatField(materialSettings.transitionPos1);
                    } EditorGUILayout.EndHorizontal();
                    DrawTPosition(ref materialSettings.transitionPos1, ref materialSettings.transitionType1);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel("Transition2 Position");
                        EditorGUILayout.FloatField(materialSettings.transitionPos2);
                    } EditorGUILayout.EndHorizontal();
                    DrawTPosition(ref materialSettings.transitionPos2, ref materialSettings.transitionType2);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel("Transition1 Type");
                        EditorGUILayout.FloatField(materialSettings.transitionType1);
                    } EditorGUILayout.EndHorizontal();
                    DrawTType(ref materialSettings.transitionType1);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel("Transition2 Type");
                        EditorGUILayout.FloatField(materialSettings.transitionType2);
                    } EditorGUILayout.EndHorizontal();
                    DrawTType(ref materialSettings.transitionType2);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                DrawT1Textures();
                GUILayout.FlexibleSpace();
                DrawT2Textures();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            materialSettings.limitMin = EditorGUILayout.FloatField("LimitMin", materialSettings.limitMin);
            materialSettings.limitMax = EditorGUILayout.FloatField("LimitMax", materialSettings.limitMax);

            thisMaterial.SetTransitionSettingsToShader(materialSettings);

        }

        private void DrawTPosition(ref float transitionPos, ref float transitionType)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    GUI.skin = _mySkin;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Button("");
                    if (GUILayout.Button("v"))
                        transitionPos = 2;
                    GUILayout.Button("");
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(">"))
                        transitionPos = 1;
                    else if (GUILayout.Button("x"))
                    {
                        transitionPos = 0;
                        transitionType = 0;
                    }
                    else if (GUILayout.Button("<"))
                        transitionPos = 3;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Button("");
                    if (GUILayout.Button("^"))
                        transitionPos = 4;
                    GUILayout.Button("");
                    EditorGUILayout.EndHorizontal();
                    GUI.skin = null;

                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTType(ref float transitionType)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();

            GUI.skin = _mySkin;
            if (GUILayout.Button(btt0))
                transitionType = 0;
            else if (GUILayout.Button(btt1))
                transitionType = 1;
            else if (GUILayout.Button(btt2))
                transitionType = 2;
            else if (GUILayout.Button(btt3))
                transitionType = 3;
            GUI.skin = null;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawT1Textures()
        {
            EditorGUILayout.BeginVertical();
            showT1Textures = EditorGUILayout.Foldout(showT1Textures, "Transition1 Textures");
            if (showT1Textures)
            {
                materialSettings.tex11 = EditorGUILayout.ObjectField("Texture1", materialSettings.tex11, typeof(Texture2D), true) as Texture2D;
                materialSettings.tex12 = EditorGUILayout.ObjectField("Texture2", materialSettings.tex12, typeof(Texture2D), true) as Texture2D;
                materialSettings.tex13 = EditorGUILayout.ObjectField("Texture3", materialSettings.tex13, typeof(Texture2D), true) as Texture2D;
                materialSettings.tex14 = EditorGUILayout.ObjectField("Texture4", materialSettings.tex14, typeof(Texture2D), true) as Texture2D;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawT2Textures()
        {
            EditorGUILayout.BeginVertical();
            showT2Textures = EditorGUILayout.Foldout(showT2Textures, "Transition2 Textures");
            if (showT2Textures)
            {
                materialSettings.tex21 = EditorGUILayout.ObjectField("Texture1", materialSettings.tex21, typeof(Texture2D), true) as Texture2D;
                materialSettings.tex22 = EditorGUILayout.ObjectField("Texture2", materialSettings.tex22, typeof(Texture2D), true) as Texture2D;
                materialSettings.tex23 = EditorGUILayout.ObjectField("Texture3", materialSettings.tex23, typeof(Texture2D), true) as Texture2D;
                materialSettings.tex24 = EditorGUILayout.ObjectField("Texture4", materialSettings.tex24, typeof(Texture2D), true) as Texture2D;
            }
            EditorGUILayout.EndVertical();
        }

    }
}
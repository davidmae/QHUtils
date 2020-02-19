using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{

    public static class BiomeSettingsInspector
    {
        public static MaterialTerrain Draw(MaterialTerrain material)
        {
            var materialSettings = material.GetMaterialSettingsFromShader();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Effects");
            materialSettings.color = EditorGUILayout.ColorField(materialSettings.color);
            EditorGUILayout.BeginHorizontal();
            materialSettings.glossines = EditorGUILayout.Slider(materialSettings.glossines, 0, 1);
            GUILayout.Label("- Smooth");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            materialSettings.metallic = EditorGUILayout.Slider(materialSettings.metallic, 0, 1);
            GUILayout.Label("- Metallic");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            materialSettings.scale = EditorGUILayout.Slider(materialSettings.scale, 0, 1);
            GUILayout.Label("- Scale");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator(); EditorGUILayout.Separator();

            materialSettings.tex1 = EditorGUILayout.ObjectField("", materialSettings.tex1, typeof(Texture2D), true) as Texture2D;
            EditorGUIUtility.labelWidth = 20;
            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
                materialSettings.nrm1 = EditorGUILayout.ObjectField("", materialSettings.nrm1, typeof(Texture2D), true) as Texture2D;

            EditorGUILayout.EndHorizontal();

            //----------------------------------------------------------------
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Texture1");
            EditorGUILayout.BeginHorizontal();
            materialSettings.tex2h = EditorGUILayout.Slider(materialSettings.tex2h, 0, 1);
            GUILayout.Label("H");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            materialSettings.tex2b = EditorGUILayout.Slider(materialSettings.tex2b, 0, 100);
            GUILayout.Label("B");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            materialSettings.tex2 = EditorGUILayout.ObjectField("", materialSettings.tex2, typeof(Texture2D), true) as Texture2D;
            EditorGUIUtility.labelWidth = 10;
            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
                materialSettings.nrm2 = EditorGUILayout.ObjectField("", materialSettings.nrm2, typeof(Texture2D), true) as Texture2D;

            EditorGUILayout.EndHorizontal();

            //----------------------------------------------------------------
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Texture2");
            EditorGUILayout.BeginHorizontal();
            materialSettings.tex3h = EditorGUILayout.Slider(materialSettings.tex3h, 0, 1);
            GUILayout.Label("H");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            materialSettings.tex3b = EditorGUILayout.Slider(materialSettings.tex3b, 0, 100);
            GUILayout.Label("B");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            materialSettings.tex3 = EditorGUILayout.ObjectField("", materialSettings.tex3, typeof(Texture2D), true) as Texture2D;
            EditorGUIUtility.labelWidth = 10;
            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
                materialSettings.nrm3 = EditorGUILayout.ObjectField("", materialSettings.nrm3, typeof(Texture2D), true) as Texture2D;

            EditorGUILayout.EndHorizontal();

            //----------------------------------------------------------------
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Texture3");
            EditorGUILayout.BeginHorizontal();
            materialSettings.tex4h = EditorGUILayout.Slider(materialSettings.tex4h, 0, 1);
            GUILayout.Label("H");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            materialSettings.tex4b = EditorGUILayout.Slider(materialSettings.tex4b, 0, 100);
            GUILayout.Label("B");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            materialSettings.tex4 = EditorGUILayout.ObjectField("", materialSettings.tex4, typeof(Texture2D), true) as Texture2D;
            EditorGUIUtility.labelWidth = 10;
            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
                materialSettings.nrm4 = EditorGUILayout.ObjectField("", materialSettings.nrm4, typeof(Texture2D), true) as Texture2D;

            EditorGUILayout.EndHorizontal();

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
            {
                //----------------------------------------------------------------

                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Texture4");
                EditorGUILayout.BeginHorizontal();
                materialSettings.tex5h = EditorGUILayout.Slider(materialSettings.tex5h, 0, 1);
                GUILayout.Label("H");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                materialSettings.tex5b = EditorGUILayout.Slider(materialSettings.tex5b, 0, 100);
                GUILayout.Label("B");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                materialSettings.tex5 = EditorGUILayout.ObjectField("", materialSettings.tex5, typeof(Texture2D), true) as Texture2D;
                EditorGUIUtility.labelWidth = 10;
                materialSettings.nrm5 = EditorGUILayout.ObjectField("", materialSettings.nrm5, typeof(Texture2D), true) as Texture2D;

                EditorGUILayout.EndHorizontal();

                //---------------------------------------------------------------

                /*EditorGUILayout.Separator();
            
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Texture5");
                EditorGUILayout.BeginHorizontal();
                materialSettings.tex6h = EditorGUILayout.Slider(materialSettings.tex6h, 0, 1);
                GUILayout.Label("H");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                materialSettings.tex6b = EditorGUILayout.Slider(materialSettings.tex6b, 0, 100);
                GUILayout.Label("B");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            
                materialSettings.tex6 = EditorGUILayout.ObjectField("", materialSettings.tex6, typeof(Texture2D), true) as Texture2D;
                EditorGUIUtility.labelWidth = 10;
                materialSettings.nrm6 = EditorGUILayout.ObjectField("", materialSettings.nrm6, typeof(Texture2D), true) as Texture2D;
            
                EditorGUILayout.EndHorizontal();*/
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label("CliffScale");
                    EditorGUILayout.BeginHorizontal();
                    materialSettings.cliffScale = EditorGUILayout.Vector2Field("", materialSettings.cliffScale);
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Label("CliffBlend");
                    materialSettings.cliffB = EditorGUILayout.Slider(materialSettings.cliffB, 0, 2);

                    GUILayout.Label("CliffFade");
                    materialSettings.cliffFade = EditorGUILayout.Slider(materialSettings.cliffFade, 0, 1);

                    GUILayout.Label("CliffMinHeight");
                    materialSettings.cliffMin = EditorGUILayout.Slider(materialSettings.cliffMin, -1, 1);

                    GUILayout.Label("CliffMaxHeight");
                    materialSettings.cliffMax = EditorGUILayout.Slider(materialSettings.cliffMax, -1, 1);

                    GUILayout.Label("CliffFadeTreshold");
                    materialSettings.cliffFadeTreshold = EditorGUILayout.Slider(materialSettings.cliffFadeTreshold, 1, 10);

                    GUILayout.Label("CliffFadeBottom");
                    materialSettings.cliffFadeBottom = EditorGUILayout.Slider(materialSettings.cliffFadeBottom, 0, 200);

                    GUILayout.Label("CliffFadeTop");
                    materialSettings.cliffFadeTop = EditorGUILayout.Slider(materialSettings.cliffFadeTop, 0, 200);

                    GUILayout.Label("SteepNes");
                    materialSettings.steepnes = EditorGUILayout.Slider(materialSettings.steepnes, 0, 1);

                    GUILayout.Label("SteepNesBlend");
                    materialSettings.steepnesBlend = EditorGUILayout.Slider(materialSettings.steepnesBlend, 0, 20);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();


                materialSettings.texCliff = EditorGUILayout.ObjectField("", materialSettings.texCliff, typeof(Texture2D), true) as Texture2D;
                EditorGUIUtility.labelWidth = 10;
                if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
                    materialSettings.nrmCliff = EditorGUILayout.ObjectField("", materialSettings.nrmCliff, typeof(Texture2D), true) as Texture2D;

            }
            EditorGUILayout.EndHorizontal();
            //----------------------------------------------------------------

            material.SetMaterialSettingsToShader(materialSettings);

            return material;
        }

    }
}
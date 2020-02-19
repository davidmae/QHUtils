using UnityEngine;
using System.Collections;

// ###################################################################################################################
// ################## This class works as bridge at shader/material and custom inspector #############################
// ################## His main pourpose is to Get/Set properties from the custom shader  #############################
// ###################################################################################################################

namespace QHLand
{

    public class MaterialTerrain
    {
        public struct MaterialSettings
        {
            public Texture2D tex1, tex2, tex3, tex4, tex5, texCliff, nrmCliff, tex11, tex12, tex13, tex14, tex21, tex22, tex23, tex24;
            public Texture2D nrm1, nrm2, nrm3, nrm4, nrm5;
            public float tex2h, tex2b, tex3h, tex3b, tex4h, tex4b, tex5h, tex5b, cliffB, cliffMin, cliffMax, steepnes, steepnesBlend, cliffFade, cliffFadeBottom, cliffFadeTop, cliffFadeTreshold, glossines, metallic, scale;
            public float transitionPos1, transitionPos2, transitionLength1;
            public float transitionType1, transitionType2, transitionLength2;
            public float limitMin, limitMax;
            public Vector2 cliffScale;
            public Color color;
        }

        public string name { get; set; }
        public Material material { get; private set; }
        public bool withoutTextures = false;

        private MaterialSettings settings;
        private string newbiome;

        //public MaterialTerrain(Material _material)
        //{
        //    material = _material;
        //    name = _material.name;
        //}

        public MaterialTerrain(string name)
        {
            string shader = (WorldManager.worldInstance.shaderType == eShaderType.Shader_OriginalMode ? Paths.MyCustomShader : Paths.MyCustomShader_NoTransition);
            material = new Material(Shader.Find(shader));
            UnityEditor.AssetDatabase.CreateAsset(material, Paths.BiomeMaterials + name + "Material.mat");
            name = material.name;
        }

        public MaterialTerrain(Material material)
        {
            this.material = material;
            name = material.name;
        }

        public void ChangeMaterialTo(Material _material)
        {
            if (material == _material)
                return;

            Shader shader = material.shader;
            material = _material;
            material.shader = shader;
            name = _material.name;
        }

        public void SetShaderType(eShaderType shader)
        {
            material.shader = Shader.Find(
                shader == eShaderType.Shader_OriginalMode ? Paths.MyCustomShader : Paths.MyCustomShader_NoTransition
                );
        }

        // This void sets the shader properties from MaterialSettings previously taken in the inspector
        public void SetMaterialSettingsToShader(MaterialSettings _settings)
        {
            material.SetFloat("_Tex2Height", _settings.tex2h);
            material.SetFloat("_Tex2Blend", _settings.tex2b);
            material.SetFloat("_Tex3Height", _settings.tex3h);
            material.SetFloat("_Tex3Blend", _settings.tex3b);
            material.SetFloat("_Tex4Height", _settings.tex4h);
            material.SetFloat("_Tex4Blend", _settings.tex4b);

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
            {
                material.SetFloat("_Tex5Height", _settings.tex5h);
                material.SetFloat("_Tex5Blend", _settings.tex5b);
            }

            material.SetColor("_Color", _settings.color);
            material.SetFloat("_Metallic", _settings.metallic);
            material.SetFloat("_Glossiness", _settings.glossines);
            material.SetFloat("_Scale", _settings.scale);

            material.SetTextureScale("_Cliff", new Vector2(_settings.cliffScale.x, _settings.cliffScale.y));
            material.SetFloat("_CliffFade", _settings.cliffFade);
            material.SetFloat("_CliffBlend", _settings.cliffB);
            material.SetFloat("_CliffMinH", _settings.cliffMin);
            material.SetFloat("_CliffMaxH", _settings.cliffMax);
            material.SetFloat("_CliffFadeBottom", _settings.cliffFadeBottom);
            material.SetFloat("_CliffFadeTop", _settings.cliffFadeTop);
            material.SetFloat("_SteepNes", _settings.steepnes);
            material.SetFloat("_SteepNesBlend", _settings.steepnesBlend);
            material.SetFloat("_CliffFadeTreshold", _settings.cliffFadeTreshold);

            material.SetTexture("_NormalTex0", _settings.nrm1);
            material.SetTexture("_NormalTex1", _settings.nrm2);
            material.SetTexture("_NormalTex2", _settings.nrm3);
            material.SetTexture("_NormalTex3", _settings.nrm4);
            material.SetTexture("_NormalTex4", _settings.nrm5);
            material.SetTexture("_CliffBump", _settings.nrmCliff);

            if (withoutTextures)
                return;

            material.SetTexture("_Texture0", _settings.tex1);
            material.SetTexture("_Texture1", _settings.tex2);
            material.SetTexture("_Texture2", _settings.tex3);
            material.SetTexture("_Texture3", _settings.tex4);

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
            {
                material.SetTexture("_Texture4", _settings.tex5);
            }

            material.SetTexture("_Cliff", _settings.texCliff);


            settings = _settings;
        }

        // This void sets the shader properties from MaterialSettings previously taken in the inspector
        public void SetTransitionSettingsToShader(MaterialSettings _settings)
        {
            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_OriginalMode)
            {
                material.SetFloat("_Transition1", _settings.transitionLength1);
                material.SetFloat("_Transition2", _settings.transitionLength2);
                material.SetFloat("_TransitionPos1", _settings.transitionPos1);
                material.SetFloat("_TransitionPos2", _settings.transitionPos2);
                material.SetFloat("_TransitionType1", _settings.transitionType1);
                material.SetFloat("_TransitionType2", _settings.transitionType2);
                material.SetFloat("_LimitMin", _settings.limitMin);
                material.SetFloat("_LimitMax", _settings.limitMax);

                material.SetTexture("_TransitionTex1", _settings.tex11);
                material.SetTexture("_TransitionTex2", _settings.tex12);
                material.SetTexture("_TransitionTex3", _settings.tex13);
                material.SetTexture("_TransitionTex4", _settings.tex14);
                material.SetTexture("_TransitionTex11", _settings.tex21);
                material.SetTexture("_TransitionTex12", _settings.tex22);
                material.SetTexture("_TransitionTex13", _settings.tex23);
                material.SetTexture("_TransitionTex14", _settings.tex24);
            }

            settings = _settings;
        }

        // This func gets the properties from shader and return a struct of all of this values.
        public MaterialSettings GetMaterialSettingsFromShader()
        {
            settings.tex2h = material.GetFloat("_Tex2Height");
            settings.tex2b = material.GetFloat("_Tex2Blend");
            settings.tex3h = material.GetFloat("_Tex3Height");
            settings.tex3b = material.GetFloat("_Tex3Blend");
            settings.tex4h = material.GetFloat("_Tex4Height");
            settings.tex4b = material.GetFloat("_Tex4Blend");

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
            {
                settings.tex5h = material.GetFloat("_Tex5Height");
                settings.tex5b = material.GetFloat("_Tex5Blend");
            }

            settings.color = material.GetColor("_Color");
            settings.metallic = material.GetFloat("_Metallic");
            settings.glossines = material.GetFloat("_Glossiness");
            settings.scale = material.GetFloat("_Scale");

            settings.tex1 = (Texture2D)material.GetTexture("_Texture0");
            settings.tex2 = (Texture2D)material.GetTexture("_Texture1");
            settings.tex3 = (Texture2D)material.GetTexture("_Texture2");
            settings.tex4 = (Texture2D)material.GetTexture("_Texture3");

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_NoTransition)
            {
                settings.tex5 = (Texture2D)material.GetTexture("_Texture4");

                settings.nrm1 = (Texture2D)material.GetTexture("_NormalTex0");
                settings.nrm2 = (Texture2D)material.GetTexture("_NormalTex1");
                settings.nrm3 = (Texture2D)material.GetTexture("_NormalTex2");
                settings.nrm4 = (Texture2D)material.GetTexture("_NormalTex3");
                settings.nrm5 = (Texture2D)material.GetTexture("_NormalTex4");

                settings.nrmCliff = (Texture2D)material.GetTexture("_CliffBump");
            }

            settings.texCliff = (Texture2D)material.GetTexture("_Cliff");


            settings.cliffScale = material.GetTextureScale("_Cliff");
            settings.cliffFade = material.GetFloat("_CliffFade");
            settings.cliffB = material.GetFloat("_CliffBlend");
            settings.cliffMin = material.GetFloat("_CliffMinH");
            settings.cliffMax = material.GetFloat("_CliffMaxH");
            settings.cliffFadeBottom = material.GetFloat("_CliffFadeBottom");
            settings.cliffFadeTop = material.GetFloat("_CliffFadeTop");
            settings.steepnes = material.GetFloat("_SteepNes");
            settings.steepnesBlend = material.GetFloat("_SteepNesBlend");
            settings.cliffFadeTreshold = material.GetFloat("_CliffFadeTreshold");

            return settings;
        }

        // This func gets the properties from shader and return a struct of all of this values.
        public MaterialSettings GetTransitionSettingsFromShader()
        {
            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_OriginalMode)
            {
                settings.transitionLength1 = material.GetFloat("_Transition1");
                settings.transitionLength2 = material.GetFloat("_Transition2");
                settings.transitionPos1 = material.GetFloat("_TransitionPos1");
                settings.transitionPos2 = material.GetFloat("_TransitionPos2");
                settings.transitionType1 = material.GetFloat("_TransitionType1");
                settings.transitionType2 = material.GetFloat("_TransitionType2");
                settings.limitMin = material.GetFloat("_LimitMin");
                settings.limitMax = material.GetFloat("_LimitMax");

                settings.tex11 = (Texture2D)material.GetTexture("_TransitionTex1");
                settings.tex12 = (Texture2D)material.GetTexture("_TransitionTex2");
                settings.tex13 = (Texture2D)material.GetTexture("_TransitionTex3");
                settings.tex14 = (Texture2D)material.GetTexture("_TransitionTex4");
                settings.tex21 = (Texture2D)material.GetTexture("_TransitionTex11");
                settings.tex22 = (Texture2D)material.GetTexture("_TransitionTex12");
                settings.tex23 = (Texture2D)material.GetTexture("_TransitionTex13");
                settings.tex24 = (Texture2D)material.GetTexture("_TransitionTex14");
            }

            return settings;
        }

        public Texture GetTexture(string name)
        {
            return material.GetTexture(name);
        }

        public void SetTexture(string name, Texture2D texture)
        {
            material.SetTexture(name, texture);
        }

        public MaterialSettings GetMaterialSettings()
        {
            return settings;
        }

        public void SetMaterial(Material material)
        {
            this.material = material;
        }
    }
}
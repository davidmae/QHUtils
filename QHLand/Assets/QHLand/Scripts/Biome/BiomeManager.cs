using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    public static class BiomeManager
    {
        public static List<Biome> biomes = new List<Biome>();

        public static void CreateNewBiome(string name)
        {
            MaterialTerrain materialTerrain = new MaterialTerrain(name);

            //string shader = (WorldManager.worldInstance.shaderType == eShaderType.Shader_OriginalMode ? PATHS.MyCustomShader : PATHS.MyCustomShader_NoTransition);
            //Material material = new Material(Shader.Find(shader));
            //
            //UnityEditor.AssetDatabase.CreateAsset(material, PATHS.BiomeMaterials + newbiome + "Material.mat");

            biomes.Add(new Biome(name, materialTerrain));
        }

        public static void FillBiomeList()
        {
            biomes.Clear();

            var files = ResourceLoader.LoadAllFilesIn(Paths.BiomeMaterials);
            foreach (var file in files)
            {
                if (file.Extension == ".mat")
                {
                    try
                    {
                        if (!file.Name.Contains("Material"))
                            continue;

                        string biomeName = file.Name.Substring(0, file.Name.Length - ("Material.mat".Length));
                        if (biomeName.Length > 0)
                        {
                            var material = LoadMaterial(biomeName);
                            biomes.Add(new Biome(biomeName, new MaterialTerrain(material)));
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning(e.Message);
                    }
                }
            }

        }

        public static Material LoadMaterial(string name)
        {
            var material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(Paths.BiomeMaterials + name + "Material.mat");
            return material;
        }

        public static void SetMaterial(int biome, Material material)
        {
            for (int i = 0; i < biomes.Count; ++i)
            {
                if (i == biome)
                    biomes[i].materialTerrain.SetMaterial(material);
            }

        }

        public static MaterialTerrain GetMaterial(int biome)
        {
            for (int i = 0; i < biomes.Count; ++i)
            {
                if (i == biome)
                    return biomes[i].materialTerrain;
            }

            return null;
        }

        //public static void SetMaterial(int biome, MaterialTerrain material)
        //{
        //    for (int i = 0; i < biomes.Count; ++i)
        //    {
        //        if (i == biome)
        //            biomes[i].SetMaterial(material);
        //    }
        //
        //}
    }
}
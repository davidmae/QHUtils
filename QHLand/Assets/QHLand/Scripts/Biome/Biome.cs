using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    public class Biome
    {
        //public static string Path = PATHS.BiomeMaterials;

        public MaterialTerrain materialTerrain { get; private set; }
        public string name { get; private set; }
        //public Material material { get; private set; }

        //public Biome(string name)
        //{
        //    this.name = name;
        //    this.material = Resources.LoadAssetAtPath<Material>(Path + name + "Material.mat");
        //}

        public Biome(string name, MaterialTerrain materialTerrain)
        {
            this.materialTerrain = materialTerrain;
            this.name = name;
        }

        //public void SetMaterial(Material material)
        //{
        //    this.material = material;
        //}

        public void SetMaterial(MaterialTerrain materialTerrain)
        {
            this.materialTerrain = materialTerrain;
        }

    }
}
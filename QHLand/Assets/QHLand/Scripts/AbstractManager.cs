using UnityEngine;
using System.Collections;

namespace QHLand
{

    public abstract class AbstractManager : MonoBehaviour
    {
        public int gridSizeY { get; set; }
        public int gridSizeX { get; set; }
        public float coastAssymetry { get; set; }
        public int coastResolution { get; set; }
        public int startCoast { get; set; }
        public int smoothCoastGrade { get; set; }
        public TerrainResolution terrainResolution { get; set; }
        public GameObject waterModel { get; set; }
        public float waterHeight { get; set; }
        public float lastWaterHeight { get; set; }
        public bool showWater { get; set; }
        public bool watercreated { get; set; }

        [HideInInspector]
        public float waterMenuFade;
        [HideInInspector]
        public float noiseMenuFade;
        [HideInInspector]
        public float edgesMenuFade;
        [HideInInspector]
        public float smoothTerrainFade;
        [HideInInspector]
        public float saveloadMenuFade;

        abstract public void GenerateGrass(int biome, int igenerator);
        abstract public void GenerateNoise(bool update);
        abstract public void GenerateBiome();
        abstract public void ChangeMaterialSettings(int biomeSelected, MaterialTerrain material, MaterialTerrain.MaterialSettings settings);
        abstract public void SmoothBorders();
        abstract public void SmoothTerrain(int iterations);
        abstract public void Save(string _path);
        abstract public void Load(string _path);


        abstract public void CreateWaterLayer(int initX, int initY, int chunksX, int chunksY);
        abstract public void DestroyWaterLayer();
        abstract public void UpdateWater();

        /*public virtual void CreateWaterLayer(int initX, int initY, int chunksX, int chunksY)
        {
            if (watercreated)
                return;

            if (waterModel == null)
                return;

            waterModel = Instantiate(waterModel, Vector3.zero, Quaternion.identity) as GameObject;

            float height = terrainResolution.terrainSize * chunksY;
            float width = terrainResolution.terrainSize * chunksX;

            waterModel.transform.position = new Vector3(width * .5f, waterHeight, height * .5f);
            waterModel.transform.localScale = new Vector3(chunksX * 5, 0f, chunksY * 5);
            watercreated = true;
        }*/
        /*public virtual void DestroyWaterLayer()
        {
            var gos = GameObject.FindGameObjectsWithTag("Water");
            foreach (var go in gos)
                DestroyImmediate(go);
            watercreated = false;
        }*/
        /*public virtual void UpdateWater()
        {
            if (watercreated == false)
                CreateWaterLayer(0, 0, 0, 0);

            if (waterHeight != lastWaterHeight && waterModel != null)
            {
                waterModel.transform.position = new Vector3(waterModel.transform.position.x, waterHeight, waterModel.transform.position.z);
                lastWaterHeight = waterHeight;
            }

            if (waterModel != null)
                waterModel.SetActive(showWater);
        }
        */


    }


}
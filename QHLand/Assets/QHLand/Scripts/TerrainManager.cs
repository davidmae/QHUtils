using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace QHLand
{

    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class TerrainManager : AbstractManager
    {
        public List<Terrain> NeighBors = new List<Terrain>();

        [HideInInspector]
        public int biome;
        [HideInInspector]
        public Terrain terrain;
        [HideInInspector]
        public bool stitched = false;
        [HideInInspector]
        public Chunk chunk;

        void Awake()
        {
            if (Application.isPlaying)
                return;

            terrain = GetComponent<Terrain>();

            biome = Random.Range(0, BiomeManager.biomes.Count);
        }


        // ###################################################################################################################
        // ############################ Methods used mostly by the Custom Editor classes #####################################
        // ###################################################################################################################

        public override void GenerateBiome()
        {
            MaterialTerrain bioMat = BiomeManager.GetMaterial(biome);
            bioMat.SetShaderType(WorldManager.worldInstance.shaderType);

            SplatPrototype[] splats = new SplatPrototype[1];
            splats[0] = new SplatPrototype();
            splats[0].texture = (Texture2D)(bioMat.GetTexture("_Texture0"));
            terrain.terrainData.splatPrototypes = splats;

            if (WorldManager.worldInstance.shaderType == eShaderType.Shader_OriginalMode)
            {
                bioMat.material.SetFloat("_Transition1", terrain.terrainData.size[0] / 2);
                bioMat.material.SetFloat("_Transition2", terrain.terrainData.size[0] / 2);
                bioMat.material.SetInt("_TransitionType1", 0);
                bioMat.material.SetInt("_TransitionType2", 0);
                bioMat.material.SetFloat("_TransitionPos0", 0);
                bioMat.material.SetFloat("_TransitionPos1", 0);
                bioMat.material.SetFloat("_LimitMax", terrain.terrainData.size[0]);
            }
            bioMat.material.SetFloat("_MapHeight", terrain.terrainData.size[1]);
            bioMat.material.SetFloat("_MapSize", terrain.terrainData.size[0]);

            terrain.materialType = Terrain.MaterialType.Custom;
            terrain.materialTemplate = (Material)Instantiate(bioMat.material);
        }
        public override void ChangeMaterialSettings(int biome, MaterialTerrain material, MaterialTerrain.MaterialSettings settings)
        {
            if (chunk.chunkTerrain.materialTemplate == null) return;

            if (chunk.chunkTerrain.materialTemplate.name.Contains(material.name) || biome == BiomeManager.biomes.Count + 1)
            {
                MaterialTerrain newMat = new MaterialTerrain(chunk.chunkTerrain.materialTemplate);
                if (biome == BiomeManager.biomes.Count + 1) newMat.withoutTextures = true;
                newMat.SetMaterialSettingsToShader(settings);
                newMat.withoutTextures = false;
                chunk.chunkTerrain.materialTemplate = newMat.material;
            }
        }
        public override void GenerateGrass(int biome, int igenerator)
        {
            if (this.biome == biome || biome == BiomeManager.biomes.Count + 1)
                GrassManager.GetGenerator(igenerator).GenerateGrass(chunk);
        }


        public override void SmoothTerrain(int iterations)
        {
            int width = terrain.terrainData.heightmapWidth;
            int height = terrain.terrainData.heightmapHeight;

            float[,] hts = terrain.terrainData.GetHeights(0, 0, width, height);

            UnityEditor.Undo.RegisterCompleteObjectUndo(chunk.chunkTerrain.terrainData, "Noise");

            while (iterations > 0)
            {
                iterations--;
                Utils.Smooth(0, 0, width, height, new float[width, height], ref hts);
            }

            terrain.terrainData.SetHeights(0, 0, hts);
        }
        public override void SmoothBorders()
        {
            int height = chunk.chunkTerrain.terrainData.heightmapHeight;
            int width = chunk.chunkTerrain.terrainData.heightmapHeight;

            int initH = (int)chunk.chunkTerrain.transform.position.z;
            int initW = (int)chunk.chunkTerrain.transform.position.x;

            int endH = initH + height;
            int endW = initW + width;

            float[,] heights = chunk.chunkTerrain.terrainData.GetHeights(0, 0, height, width);

            UnityEditor.Undo.RegisterCompleteObjectUndo(chunk.chunkTerrain.terrainData, "Noise");

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    heights[i, j] *= ((j < initW + startCoast || j > endW - startCoast || i < initH + startCoast || i > endH - startCoast) ? 0f : 1f);
                }
            }

            if (NeighBors[(int)eSides.BOTTOM] == null)
                Utils.SmoothBorder(eSides.BOTTOM, chunk.chunkTerrain, ref heights, startCoast);
            if (NeighBors[(int)eSides.LEFT] == null)
                Utils.SmoothBorder(eSides.LEFT, chunk.chunkTerrain, ref heights, startCoast);
            if (NeighBors[(int)eSides.RIGHT] == null)
                Utils.SmoothBorder(eSides.RIGHT, chunk.chunkTerrain, ref heights, startCoast);
            if (NeighBors[(int)eSides.UP] == null)
                Utils.SmoothBorder(eSides.UP, chunk.chunkTerrain, ref heights, startCoast);

            chunk.chunkTerrain.terrainData.SetHeights(0, 0, heights);
        }

        public void SetNeighBors(List<Chunk> neighbors)
        {
            // Set value of adjacent neighbor if isn't null, in this case is setted to null.
            // Used by UpdateTerrainNeighbors() - WorldManager
            foreach (Chunk terrain in neighbors)
            {
                try
                {
                    NeighBors.Add(terrain.chunkTerrain);
                }
                catch
                {
                    NeighBors.Add(null);
                }
            }
        }


        // ###########################################
        // throw new System.NotImplementedException();
        public override void Save(string _path)
        {
            throw new System.NotImplementedException();
        }
        public override void Load(string _path)
        {
            throw new System.NotImplementedException();
        }
        public override void GenerateNoise(bool update)
        {
            throw new System.NotImplementedException();
        }
        public override void CreateWaterLayer(int initX, int initY, int chunksX, int chunksY)
        {
            throw new System.NotImplementedException();
        }
        public override void DestroyWaterLayer()
        {
            throw new System.NotImplementedException();
        }
        public override void UpdateWater()
        {
            throw new System.NotImplementedException();
        }
        // ###########################################


        // ###################################################################################################################
        // ################ The data is serializaed into custom types for save tasks #########################################
        // ###################################################################################################################

        public CTYPES.CHUNK_DATA Serialize()
        {
            Terrain chunk = GetComponent<Terrain>();

            CTYPES.FLOAT3 pos = new CTYPES.FLOAT3(transform.position.x, chunk.transform.position.y, chunk.transform.position.z);
            float[,] hts = chunk.terrainData.GetHeights(0, 0, chunk.terrainData.heightmapWidth, chunk.terrainData.heightmapHeight);

            List<string> neighbors = new List<string>();

            foreach (Terrain neigh in NeighBors)
            {
                if (neigh == null)
                    neighbors.Add(null);
                else
                    neighbors.Add(neigh.name);
            }

            return new CTYPES.CHUNK_DATA(name, pos, hts, biome, neighbors);
        }


        // ###################################################################################################################
        // ############### Load method #######################################################################################
        // ###################################################################################################################

        public void Load(CTYPES.CHUNK_DATA chunkData)
        {
            Terrain[] childs = transform.parent.GetComponentsInChildren<Terrain>();

            biome = chunkData.biome;
            GenerateBiome();

            foreach (string neighbor in chunkData.neighBors)
            {
                if (neighbor == null)
                {
                    NeighBors.Add(null);
                }
                else
                {
                    bool found = false; int i = 0;
                    while (!found)
                    {
                        if (neighbor == childs[i].name)
                        {
                            NeighBors.Add(childs[i]);
                            found = true;
                        }
                        else ++i;
                    }
                }
            }

        }



    }
}
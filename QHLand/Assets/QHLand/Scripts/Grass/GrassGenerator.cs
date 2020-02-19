using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    [System.Serializable]
    public class GrassGenerator
    {
        public List<GrassDataLayer> grassLayers;

        public GrassGenerator()
        {
            grassLayers = new List<GrassDataLayer>();
        }

        public void GenerateGrass(Chunk chunk)
        {
            int alphamapWidth = chunk.chunkTerrain.terrainData.alphamapWidth;
            int alphamapHeight = chunk.chunkTerrain.terrainData.alphamapHeight;
            int detailWidth = chunk.chunkTerrain.terrainData.detailResolution;
            int detailHeight = detailWidth;

            float resolutionDiffFactor = (float)alphamapWidth / detailWidth;


            DetailPrototype[] details = new DetailPrototype[grassLayers.Count];
            for (int i = 0; i < grassLayers.Count; ++i)
            {
                details[i] = new DetailPrototype();
                details[i].usePrototypeMesh = grassLayers[i].usePrototypeMesh;

                if (details[i].usePrototypeMesh)
                {
                    details[i].prototype = grassLayers[i].detailMesh;
                    details[i].renderMode = DetailRenderMode.Grass;
                }
                else
                {
                    details[i].prototypeTexture = grassLayers[i].detailTexture;
                    details[i].renderMode = DetailRenderMode.GrassBillboard;
                }

                details[i].minHeight = grassLayers[i].details.minHeight;
                details[i].maxHeight = grassLayers[i].details.maxHeight;
                details[i].minWidth = grassLayers[i].details.minWidth;
                details[i].maxWidth = grassLayers[i].details.maxWidth;
                details[i].dryColor = grassLayers[i].details.dryColor;
                details[i].healthyColor = grassLayers[i].details.healthyColor;
            }
            chunk.chunkTerrain.terrainData.detailPrototypes = details;


            float[, ,] splatmap = chunk.chunkTerrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);

            UnityEditor.Undo.RegisterCompleteObjectUndo(chunk.chunkTerrain.terrainData, "Grass");

            for (int i = 0; i < grassLayers.Count; ++i)
            {
                GrassDataLayer grassLayer = grassLayers[i];

                if (grassLayer.disable == true)
                    continue;

                int[,] newDetailLayer = new int[detailWidth, detailHeight];

                float seedX = grassLayer.seed + chunk.pos.x;
                float seedY = grassLayer.seed + chunk.pos.y;

                for (int j = 0; j < detailWidth; j++)
                {
                    float nj = (float)j / (float)detailWidth;

                    for (int k = 0; k < detailHeight; k++)
                    {
                        float nk = (float)k / (float)detailHeight;

                        float height = chunk.chunkTerrain.terrainData.GetInterpolatedHeight(nk, nj);
                        float steepness = chunk.chunkTerrain.terrainData.GetSteepness(nk, nj);

                        if (height < grassLayer.minSpawnH || height > grassLayer.maxSpawnH)
                            continue;

                        if (steepness < grassLayer.minSlope || steepness > grassLayer.maxSlope)
                            continue;

                        float perlin = Mathf.PerlinNoise((seedX + nk) * grassLayer.frequency, (seedY + nj) * grassLayer.frequency);

                        if (grassLayer.inverse ? perlin >= grassLayer.perlinTreshold : perlin < grassLayer.perlinTreshold)
                            continue;

                        float alphaValue = splatmap[(int)(resolutionDiffFactor * j), (int)(resolutionDiffFactor * k), 0];

                        newDetailLayer[j, k] = (int)Mathf.Round(alphaValue * ((float)grassLayer.detailCountPerDetailPixel)) + newDetailLayer[j, k];
                    }
                }

                chunk.chunkTerrain.terrainData.SetDetailLayer(0, 0, i, newDetailLayer);

            }
        }

    }

    [System.Serializable]
    public class GrassDataLayer
    {
        public bool usePrototypeMesh = false;
        [System.NonSerialized]
        public Texture2D detailTexture;
        [System.NonSerialized]
        public GameObject detailMesh;
        public string detailTexturePath;
        public string detailMeshPath;
        public int detailCountPerDetailPixel = 5;
        public float minSpawnH = 0;
        public float maxSpawnH = 100;
        public float minSlope = 0;
        public float maxSlope = 30;

        [Range(0, 1)]
        public float perlinTreshold = 0.5f;
        public bool inverse = false;
        public float frequency = 5f;
        public float seed = 1f;
        public bool disable = false;

        public DetailPrototypeCustom details;

        public GrassDataLayer()
        {
            details = new DetailPrototypeCustom();
        }

        public GrassDataLayer(GrassDataLayer gdl)
        {
            usePrototypeMesh = gdl.usePrototypeMesh;
            detailTexture = gdl.detailTexture;
            detailMesh = gdl.detailMesh;
            detailCountPerDetailPixel = gdl.detailCountPerDetailPixel;
            minSpawnH = gdl.minSpawnH;
            maxSpawnH = gdl.maxSpawnH;
            minSlope = gdl.minSlope;
            maxSlope = gdl.maxSlope;
            perlinTreshold = gdl.perlinTreshold;
            inverse = gdl.inverse;
            frequency = gdl.frequency;
            seed = gdl.seed;
            disable = gdl.disable;

            detailTexturePath = gdl.detailTexturePath;
            detailMeshPath = gdl.detailMeshPath;

            details = new DetailPrototypeCustom(gdl.details);
        }

    }

    [System.Serializable]
    public class DetailPrototypeCustom
    {
        public float minHeight = 1f;
        public float maxHeight = 1f;
        public float minWidth = 1f;
        public float maxWidth = 1f;
        [System.NonSerialized]
        public Color dryColor = Color.green;
        [System.NonSerialized]
        public Color healthyColor = Color.green;
        public CTYPES.COLOR cdryColor;
        public CTYPES.COLOR chealthyColor;

        public DetailPrototypeCustom()
        { }

        public DetailPrototypeCustom(DetailPrototypeCustom dpc)
        {
            minHeight = dpc.minHeight;
            maxHeight = dpc.maxHeight;
            minWidth = dpc.minWidth;
            maxWidth = dpc.maxWidth;
            dryColor = dpc.dryColor;
            healthyColor = dpc.healthyColor;

            cdryColor = dpc.cdryColor;
            chealthyColor = dpc.chealthyColor;
        }

    }
}
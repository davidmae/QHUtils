using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    public static class NoiseManager
    {
        public static List<NoiseLayer> noiseLayers = new List<NoiseLayer>();
        public static int currLayer = -1;


        public static NoiseLayer Next()
        {
            if (noiseLayers.Count == 0)
            {
                currLayer = -1;
                return null;
            }

            currLayer = (++currLayer) % noiseLayers.Count;
            return (noiseLayers[currLayer]);
        }

        public static void Save(string path)
        {
            List<CTYPES.NOISE_LAYER> savedObject = new List<CTYPES.NOISE_LAYER>();

            foreach (NoiseLayer layer in noiseLayers)
            {
                savedObject.Add(new CTYPES.NOISE_LAYER(layer.noiseData, new CTYPES.FLOAT2(layer.seed.x, layer.seed.y), layer.seedIgnore, layer.islandMode));
            }

            SaveHandler.saveObject = savedObject;
            SaveHandler.Save(path, true);
        }

        public static void Load(string path)
        {
            if (noiseLayers.Count > 0)
                noiseLayers.Clear();

            SaveHandler.Load(path, true);
            List<CTYPES.NOISE_LAYER> loadedObject = (List<CTYPES.NOISE_LAYER>)(SaveHandler.deserializedObject);

            foreach (CTYPES.NOISE_LAYER obj in loadedObject)
            {
                noiseLayers.Add(new NoiseLayer(obj.noiseData, new Vector2(obj.seed.x, obj.seed.y), obj.seedIgnore, obj.islandMode));
            }

            Next();
        }

    }


    public class NoiseLayer
    {
        public NoiseData noiseData;
        public Vector2 seed;
        public bool seedIgnore;
        public bool islandMode;

        public NoiseLayer()
        {
            noiseData = new NoiseData();

            seed.x = Random.value * 10f;
            seed.y = Random.value * 10f;

            islandMode = false;

            seedIgnore = false;
        }

        public NoiseLayer(NoiseData noiseData, Vector2 seed, bool seedIgnore, bool islandMode)
        {
            this.noiseData = noiseData;
            this.seed = seed;
            this.seedIgnore = seedIgnore;
            this.islandMode = islandMode;
        }

        public float[] GetData()
        {
            return new float[] { noiseData.octaves, noiseData.persistence, noiseData.lacunarity, noiseData.frequency };
        }

        public float CalculateNoiseValue(NoiseAlgorithm noiseAlgorithm, float i, float j, Terrain chunk, float[] args)
        {
            float noiseValue = 0.0f;

            switch (noiseData.type)
            {
                case eNoise.FRACTAL:
                    {
                        noiseValue = (float)(noiseAlgorithm.RidgedMultiFractal((seed.x + (float)i / (float)chunk.terrainData.heightmapHeight),
                                                        (seed.y + (float)j / (float)chunk.terrainData.heightmapWidth),
                                                        (int)args[0], args[1], args[2], args[3])) / noiseData.size;
                        break;
                    }
                case eNoise.FBM:
                    {
                        noiseValue = (float)(noiseAlgorithm.FractionalBrownianMotion((seed.x + (float)i / (float)chunk.terrainData.heightmapHeight),
                                                        (seed.y + (float)j / (float)chunk.terrainData.heightmapWidth),
                                                        (int)args[0], args[1], args[2], args[3])) / noiseData.size;
                        break;
                    }
                case eNoise.BILLOW:
                    {
                        noiseValue = (float)(noiseAlgorithm.Billow((seed.x + (float)i / (float)chunk.terrainData.heightmapHeight),
                                                        (seed.y + (float)j / (float)chunk.terrainData.heightmapWidth),
                                                        (int)args[0], args[1], args[2], args[3])) / noiseData.size;
                        break;
                    }
            }

            return noiseValue;
        }

    }


    [System.Serializable]
    public class NoiseData
    {
        public eNoise type;
        public string presetName;
        public int octaves;
        public float persistence;
        public float lacunarity;
        public float frequency;
        public float size;
    }

}
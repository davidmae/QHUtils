using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    [System.Serializable]
    public class TerrainResolution
    {
        public int terrainSize = 250; //width = length
        public int terrainHeight = 75;
        public int heightMapResolution = 500;
        public int detailResolution = 512;
        public int resolutionPerPatch = 15;
        public int detailObjectDistance = 250;
        public float detailObjectDensity = 0.3f;
    }

    [System.Serializable]
    public class States
    {
        public bool builded;
        public bool texturized;
        public bool noised;
        public bool stitched;
        public bool transitioned;
    }

    public static class CTYPES
    {
        [System.Serializable]
        public struct FLOAT2
        {
            public float x, y;

            public FLOAT2(float _x, float _y)
            {
                x = _x;
                y = _y;
            }
        }

        [System.Serializable]
        public struct NOISE_LAYER
        {
            public NoiseData noiseData;
            public FLOAT2 seed;
            public bool seedIgnore;
            public bool islandMode;

            public NOISE_LAYER(NoiseData _noiseData, FLOAT2 _seed, bool _seedIgnore, bool _islandMode)
            {
                noiseData = _noiseData;
                seed = _seed;
                seedIgnore = _seedIgnore;
                islandMode = _islandMode;
            }
        }

        [System.Serializable]
        public struct FLOAT3
        {
            public float x, y, z;

            public FLOAT3(float _x, float _y, float _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }
        }

        [System.Serializable]
        public struct TERRAIN_DATA
        {
            public TerrainResolution resolution;
            public int nGridsX;
            public int nGridsY;

            public TERRAIN_DATA(TerrainResolution _resolution, int gridsX, int gridsY)
            {
                resolution = _resolution;
                nGridsX = gridsX;
                nGridsY = gridsY;
            }
        }

        [System.Serializable]
        public struct CHUNK_DATA
        {
            public string name;
            public FLOAT3 pos;
            public float[,] heights;
            public int biome;
            public List<string> neighBors;

            public CHUNK_DATA(string _name, FLOAT3 _pos, float[,] _heights, int _biome, List<string> neigh)
            {
                name = _name;
                pos = _pos;
                heights = _heights;
                biome = _biome;
                neighBors = neigh;
            }
        }

        [System.Serializable]
        public struct WORLD_DATA
        {
            public TERRAIN_DATA terrainData;
            public List<CHUNK_DATA> _chunks;
            public WORLD_STATE state;

            public WORLD_DATA(TERRAIN_DATA td, List<CHUNK_DATA> c, WORLD_STATE state)
            {
                terrainData = td;
                _chunks = c;
                this.state = state;
            }
        }

        [System.Serializable]
        public struct WORLD_STATE
        {
            public bool builded, texturized, noised, stitched, transitioned;

            public WORLD_STATE(bool builded, bool texturized, bool noised, bool stitched, bool transitioned)
            {
                this.builded = builded;
                this.texturized = texturized;
                this.noised = noised;
                this.stitched = stitched;
                this.transitioned = transitioned;
            }
        }

        [System.Serializable]
        public struct BIOME_DATA
        {
            public int biome;
            public string[] textures;
            public float[][] stops;

            public BIOME_DATA(int _biome, string[] _textures, float[][] _stops)
            {
                biome = _biome;
                textures = _textures;
                stops = _stops;
            }
        }

        [System.Serializable]
        public struct COLOR
        {
            public float r, g, b, a;

            public COLOR(float r, float g, float b, float a)
            {
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }
        }

        //[System.Serializable]
        //public struct TEXTURE2D
        //{
        //    public byte[] detailTextureJPGEncode;
        //    public int detailTextureWidth;
        //    public int detailTextureHeight;
        //
        //    public TEXTURE2D(byte[] enc, int width, int height)
        //    {
        //        detailTextureJPGEncode = enc;
        //        detailTextureWidth = width;
        //        detailTextureHeight = height;
        //    }
        //}

    }


}
using UnityEngine;
using System.Collections;

namespace QHLand
{

    public class Stitcher
    {

        private static Stitcher instance;

        private Stitcher() { }

        public static Stitcher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Stitcher();
                }
                return instance;
            }
        }

        public void StitchingChunks()
        {
            Chunk[,] chunks = WorldManager.worldInstance._chunks;
            Terrain heightTerrain = chunks[chunks.GetLength(0) - 1, chunks.GetLength(1) - 1].chunkTerrain;
            Terrain terrain_aux = heightTerrain;

            for (int i = chunks.GetLength(1); i > 0; --i)
            {
                for (int j = chunks.GetLength(0); j > 0; --j)
                {
                    Terrain neighTerrain = heightTerrain.GetComponent<TerrainManager>().NeighBors[(int)eSides.BOTTOM];
                    if (neighTerrain != null)
                    {
                        StitchChunkBottom(heightTerrain, neighTerrain);
                        neighTerrain.GetComponent<TerrainManager>().stitched = true;

                        if (j == chunks.GetLength(0))
                            terrain_aux = heightTerrain.GetComponent<TerrainManager>().NeighBors[(int)eSides.BOTTOM];
                    }

                    neighTerrain = heightTerrain.GetComponent<TerrainManager>().NeighBors[(int)eSides.LEFT];
                    if (neighTerrain != null)
                    {
                        StitchChunkLeft(heightTerrain, neighTerrain);
                        neighTerrain.GetComponent<TerrainManager>().stitched = true;
                    }
                    else
                    {
                        //heightTerrain.materialTemplate.SetFloat("_Transition", 0);
                        continue;
                    }

                    heightTerrain = neighTerrain;
                }

                heightTerrain = terrain_aux;
            }

        }

        private float[,] ApplyStitching(float[] stitchs, float midHeight, float[,] hts, int j, int init, int end, eSides side)
        {
            int n = 0;

            for (int i = init; i < end; ++i)
            {
                if (side == eSides.LEFT)
                    hts[j, i] = stitchs[n];
                else
                    hts[i, j] = stitchs[n];

                n++;
            }
            return hts;
        }

        private void StitchChunkLeft(Terrain heightTerrain, Terrain neighTerrain)
        {
            float[,] thisHeights = heightTerrain.terrainData.GetHeights(0, 0, heightTerrain.terrainData.heightmapWidth, heightTerrain.terrainData.heightmapHeight);
            float[,] neighHeights = neighTerrain.terrainData.GetHeights(0, 0, neighTerrain.terrainData.heightmapWidth, neighTerrain.terrainData.heightmapHeight);

            int k = 2;
            int size = heightTerrain.terrainData.heightmapHeight;
            int tam = neighTerrain.terrainData.heightmapWidth - k;

            float[] stitch_values = new float[k];

            float thisHeight = 0f;
            float neighHeight = 0f;
            float midHeight = 0f;

            for (int j = 0; j < size; ++j)
            {
                thisHeight = thisHeights[j, k];
                neighHeight = neighHeights[j, tam];
                midHeight = Mathf.Min(thisHeight, neighHeight) + (Mathf.Abs((thisHeight - neighHeight) / 2f));

                stitch_values[0] = midHeight;
                stitch_values[1] = Mathf.Min(midHeight, thisHeight) + (Mathf.Abs((thisHeight - midHeight) / 2f));
                thisHeights = ApplyStitching(stitch_values, midHeight, thisHeights, j, 0, k, eSides.LEFT);

                stitch_values[0] = Mathf.Min(midHeight, neighHeight) + (Mathf.Abs((neighHeight - midHeight) / 2f));
                stitch_values[1] = midHeight;
                neighHeights = ApplyStitching(stitch_values, midHeight, neighHeights, j, tam, tam + k, eSides.LEFT);
            }

            neighTerrain.terrainData.SetHeights(0, 0, neighHeights);
            heightTerrain.terrainData.SetHeights(0, 0, thisHeights);
        }

        private void StitchChunkBottom(Terrain heightTerrain, Terrain neighTerrain)
        {
            float[,] thisHeights = heightTerrain.terrainData.GetHeights(0, 0, heightTerrain.terrainData.heightmapWidth, heightTerrain.terrainData.heightmapHeight);
            float[,] neighHeights = neighTerrain.terrainData.GetHeights(0, 0, neighTerrain.terrainData.heightmapWidth, neighTerrain.terrainData.heightmapHeight);

            int k = 2;
            int size = heightTerrain.terrainData.heightmapWidth;
            int tam = neighTerrain.terrainData.heightmapHeight - k;

            float[] stitch_values = new float[k];

            float thisHeight = 0f;
            float neighHeight = 0f;
            float midHeight = 0f;

            for (int j = 0; j < size; ++j)
            {
                thisHeight = thisHeights[k, j];
                neighHeight = neighHeights[tam, j];
                midHeight = Mathf.Min(thisHeight, neighHeight) + (Mathf.Abs((thisHeight - neighHeight) / 2f));

                stitch_values[0] = midHeight;
                stitch_values[1] = Mathf.Min(midHeight, thisHeight) + (Mathf.Abs((thisHeight - midHeight) / 2f));
                thisHeights = ApplyStitching(stitch_values, midHeight, thisHeights, j, 0, k, eSides.BOTTOM);

                stitch_values[0] = Mathf.Min(midHeight, neighHeight) + (Mathf.Abs((neighHeight - midHeight) / 2f));
                stitch_values[1] = midHeight;
                neighHeights = ApplyStitching(stitch_values, midHeight, neighHeights, j, tam, tam + k, eSides.BOTTOM);
            }

            neighTerrain.terrainData.SetHeights(0, 0, neighHeights);
            heightTerrain.terrainData.SetHeights(0, 0, thisHeights);
        }

    }

}
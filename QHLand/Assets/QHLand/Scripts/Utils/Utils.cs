using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    public static class Utils
    {
        public static Vector3 GetRandPoint(Vector3 init, Vector3 end)
        {
            Vector3 randPos;

            randPos.x = Random.Range(init.x, end.x);
            randPos.y = Random.Range(init.y, end.y);
            randPos.z = Random.Range(init.z, end.z);

            return randPos;
        }

        public static void RandomizeArray<T>(ref List<T> arr)
        {
            for (var i = arr.Count - 1; i > 0; i--)
            {
                var r = Random.Range(0, i);
                var tmp = arr[i];
                arr[i] = arr[r];
                arr[r] = tmp;
            }
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value);
        }

        public static void Smooth(int ix, int iy, int height, int width, float[,] new_hts, ref float[,] hts)
        {
            for (int x = ix; x < width; x++)
            {
                for (int y = iy; y < height; y++)
                {
                    int adjacents = 0;
                    float adjacentsSumHeightValue = 0.0f;

                    if ((x - 1) > 0) // Check to left
                    {
                        adjacentsSumHeightValue += hts[x - 1, y];
                        adjacents++;

                        if ((y - 1) > 0) // Check up and to the left
                        {
                            adjacentsSumHeightValue += hts[x - 1, y - 1];
                            adjacents++;
                        }

                        if ((y + 1) < height) // Check down and to the left
                        {
                            adjacentsSumHeightValue += hts[x - 1, y + 1];
                            adjacents++;
                        }
                    }

                    if ((x + 1) < width) // Check to right
                    {
                        adjacentsSumHeightValue += hts[x + 1, y];
                        adjacents++;

                        if ((y - 1) > 0) // Check up and to the right
                        {
                            adjacentsSumHeightValue += hts[x + 1, y - 1];
                            adjacents++;
                        }

                        if ((y + 1) < height) // Check down and to the right
                        {
                            adjacentsSumHeightValue += hts[x + 1, y + 1];
                            adjacents++;
                        }
                    }

                    if ((y - 1) > 0) // Check above
                    {
                        adjacentsSumHeightValue += hts[x, y - 1];
                        adjacents++;
                    }

                    if ((y + 1) < height) // Check below
                    {
                        adjacentsSumHeightValue += hts[x, y + 1];
                        adjacents++;
                    }

                    new_hts[x, y] = (hts[x, y] + (adjacentsSumHeightValue / adjacents)) * 0.5f;
                }
            }

            hts = new_hts;
        }

        // Apply a gaussian smooth to map borders
        public static void SmoothBorder(eSides side, Terrain chunk, ref float[,] heights, int startCoast)
        {
            if (side == eSides.BOTTOM)
            {
                for (int j = 0; j < chunk.terrainData.heightmapWidth; ++j)
                {
                    float ro = heights[startCoast + 1, j];
                    heights = GaussianSmoother.Instance.BOTTOM(heights, 3.25f, ro, startCoast, j, 0, 0f, 0.015f);
                }
            }
            else if (side == eSides.LEFT)
            {
                for (int j = 0; j < chunk.terrainData.heightmapHeight; ++j)
                {
                    float ro = heights[j, startCoast + 1];
                    heights = GaussianSmoother.Instance.LEFT(heights, 3.25f, ro, startCoast, j, 0, 0f, 0.015f);
                }
            }
            else if (side == eSides.UP)
            {
                for (int j = 0; j < chunk.terrainData.heightmapWidth; ++j)
                {
                    float ro = heights[chunk.terrainData.heightmapHeight - startCoast - 1, j];
                    heights = GaussianSmoother.Instance.UP(heights, 3.25f, ro, chunk.terrainData.heightmapHeight - startCoast, j, 0, 0f, 0.015f);
                }
            }
            else if (side == eSides.RIGHT)
            {
                for (int j = 0; j < chunk.terrainData.heightmapHeight; ++j)
                {
                    float ro = heights[j, chunk.terrainData.heightmapWidth - startCoast - 1];
                    heights = GaussianSmoother.Instance.RIGHT(heights, 3.25f, ro, chunk.terrainData.heightmapWidth - startCoast, j, 0, 0f, 0.015f);
                }
            }
        }

        public static void StitchingChunks()
        {
            Stitcher.Instance.StitchingChunks();
        }

        public static void DrawTransitions()
        {
            Transitioner.Instance.DrawTransitions();
        }

    }

}
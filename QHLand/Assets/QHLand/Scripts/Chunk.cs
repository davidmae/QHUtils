using UnityEngine;
using System.Collections;
using UnityEditor;

namespace QHLand
{

    // ################################################################################################
    // ########### Class for represent a terrain like a terrain-chunk. ################################
    // ################################################################################################
    public class Chunk
    {
        public Terrain chunkTerrain;
        public Vector2 pos;

        public Chunk() { }

        public Chunk(int x, int y)
        {
            var world = WorldManager.worldInstance;

            // Needed for create terrainData correctly
            TerrainData newData = new TerrainData();
            AssetDatabase.CreateAsset(newData, Paths.SavedWorlds + "Terrains/" + world.TerrainName + ((x * world.chunksY) + y));

            // Build terrain-chunk with associate data, naming and parenting
            chunkTerrain = Terrain.CreateTerrainGameObject(newData).GetComponent<Terrain>();
            chunkTerrain.name = world.TerrainName + ((x * world.chunksY) + y);
            chunkTerrain.transform.parent = world.gameObject.transform;

            // Sets the resolution for each chunk, and assign the terrain manager for each chunk
            SetResolution(world.terrainResolution);
            chunkTerrain.gameObject.AddComponent<TerrainManager>().chunk = this;

            pos = new Vector2(x, y);
        }

        public void SetResolution(TerrainResolution terrainResolution)
        {
            chunkTerrain.terrainData.heightmapResolution = terrainResolution.heightMapResolution;
            chunkTerrain.terrainData.size = new Vector3(terrainResolution.terrainSize, terrainResolution.terrainHeight, terrainResolution.terrainSize);
            chunkTerrain.terrainData.SetDetailResolution(terrainResolution.detailResolution, terrainResolution.resolutionPerPatch);
            chunkTerrain.detailObjectDensity = terrainResolution.detailObjectDensity;
            chunkTerrain.detailObjectDistance = terrainResolution.detailObjectDistance;
        }

        // Method for change shader-transition attributes for associate chunk material.
        // Used by "Transitiones.cs"
        public void SetMaterialTransition(Material matOrigin, int dir, int transitionType)
        {
            Material matTarget = chunkTerrain.materialTemplate;

            if (matTarget.GetFloat("_TransitionPos1") == 0)
            {
                matTarget.SetFloat("_TransitionPos1", dir);
                matTarget.SetFloat("_TransitionType1", transitionType);

                matTarget.SetTexture("_TransitionTex1", matOrigin.GetTexture("_Texture0"));

                matTarget.SetTexture("_TransitionTex2", matOrigin.GetTexture("_Texture1"));
                matTarget.SetTexture("_TransitionTex3", matOrigin.GetTexture("_Texture2"));
                matTarget.SetTexture("_TransitionTex4", matOrigin.GetTexture("_Texture3"));
            }
            else if (matTarget.GetFloat("_TransitionPos2") == 0)
            {
                matTarget.SetFloat("_TransitionPos2", dir);
                matTarget.SetFloat("_TransitionType2", transitionType);


                matTarget.SetTexture("_TransitionTex11", matOrigin.GetTexture("_Texture0"));

                matTarget.SetTexture("_TransitionTex12", matOrigin.GetTexture("_Texture1"));
                matTarget.SetTexture("_TransitionTex13", matOrigin.GetTexture("_Texture2"));
                matTarget.SetTexture("_TransitionTex14", matOrigin.GetTexture("_Texture3"));
            }

            matTarget.SetFloat("_LimitMin", 0);
            matTarget.SetFloat("_LimitMax", chunkTerrain.terrainData.size[0]);


            chunkTerrain.materialTemplate = matTarget;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{
    public static class GrassManager
    {
        private static List<GrassGenerator> grassGenerators = new List<GrassGenerator>();

        public static GrassGenerator GetGenerator(int generator)
        {
            return grassGenerators[generator];
        }

        public static List<GrassGenerator> GetGenerators()
        {
            return grassGenerators;
        }

        public static void Save(string path)
        {
            foreach (GrassGenerator generator in grassGenerators)
            {
                foreach (GrassDataLayer layer in generator.grassLayers)
                {
                    layer.detailTexturePath = UnityEditor.AssetDatabase.GetAssetPath(layer.detailTexture);
                    layer.detailMeshPath = UnityEditor.AssetDatabase.GetAssetPath(layer.detailMesh);

                    layer.details.cdryColor = new CTYPES.COLOR(layer.details.dryColor.r,
                                                               layer.details.dryColor.g,
                                                               layer.details.dryColor.b,
                                                               layer.details.dryColor.a);

                    layer.details.chealthyColor = new CTYPES.COLOR(layer.details.healthyColor.r,
                                                                  layer.details.healthyColor.g,
                                                                  layer.details.healthyColor.b,
                                                                  layer.details.healthyColor.a);
                }
            }

            SaveHandler.saveObject = grassGenerators;
            SaveHandler.Save(path, true);
        }

        public static void Load(string path)
        {
            SaveHandler.Load(path, true);
            grassGenerators = (List<GrassGenerator>)SaveHandler.deserializedObject;

            foreach (GrassGenerator generator in grassGenerators)
            {
                foreach (GrassDataLayer layer in generator.grassLayers)
                {
                    layer.detailTexture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(layer.detailTexturePath, typeof(Texture2D));
                    layer.detailMesh = (GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath(layer.detailMeshPath, typeof(GameObject));
                    layer.details.dryColor = new Color(layer.details.cdryColor.r,
                                                        layer.details.cdryColor.g,
                                                        layer.details.cdryColor.b,
                                                        layer.details.cdryColor.a);
                    layer.details.healthyColor = new Color(layer.details.chealthyColor.r,
                                                            layer.details.chealthyColor.g,
                                                            layer.details.chealthyColor.b,
                                                            layer.details.chealthyColor.a);
                }
            }
        }

    }

}
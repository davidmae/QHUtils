using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace QHLand
{
    public static class EditorUtils
    {
        public static Color Blue1 = new Color(.729f, .898f, .952f);


        public static void Enum(ref string[] biomes_str, ref int[] biomes_int)
        {
            //biomes_str = new string[BiomeManager.biomes.Count + 1];
            //biomes_int = new int[BiomeManager.biomes.Count + 1];

            for (int i = 0; i < biomes_str.Length - 1; ++i)
            {
                biomes_str[i] = BiomeManager.biomes[i].name;
                biomes_int[i] = i;
            }
        }

        public static void Enum<T>(ref string[] opt_str, ref int[] opt_int, int size, List<T> options)
        {
            opt_str = new string[size];
            opt_int = new int[size];

            if (typeof(T) == typeof(Biome))
            {
                Enum(ref opt_str, ref opt_int);
                return;
            }

            for (int i = 0; i < options.Count; ++i)
            {
                opt_str[i] = options[i].ToString();
                opt_int[i] = i;
            }

        }

        public static bool ButtonPressed(string buttonName, ref float vfade, Color target, Color init)
        {
            bool res = false;

            if (vfade == 0f)
                GUI.color = init;
            else
                GUI.color = target;

            if (GUILayout.Button(buttonName))
                vfade = (vfade == 0f ? System.DateTime.Now.Millisecond * 0.00001f : 0f);

            if (vfade > 0f)
                res = true;

            GUI.color = init;
            return res;
        }

        public static bool ButtonPressed(PrettyButton bt, ref float vfade, Color target, Color init)
        {
            bool res = false;

            if (vfade == 0f)
                GUI.color = init;
            else
                GUI.color = target;

            if (vfade == 0f)
                bt.ShowFade(init, target, System.DateTime.Now.Millisecond * 0.00001f);

            if (GUILayout.Button(bt.name))
                vfade = (vfade == 0f ? System.DateTime.Now.Millisecond * 0.0001f : 0f);

            if (vfade > 0f)
                res = true;

            GUI.color = init;
            return res;
        }


    }
}
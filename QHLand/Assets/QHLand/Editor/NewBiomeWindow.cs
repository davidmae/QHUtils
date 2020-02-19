using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{
    public class NewBiomeWindow : EditorWindow
    {
        string newbiome = "";

        public void Init()
        {
            EditorWindow.GetWindow<NewBiomeWindow>();
        }

        void OnGUI()
        {
            newbiome = EditorGUILayout.TextArea(newbiome);
            if (GUILayout.Button("Add"))
            {
                BiomeManager.CreateNewBiome(newbiome);
                Close();
            }
        }



    }
}
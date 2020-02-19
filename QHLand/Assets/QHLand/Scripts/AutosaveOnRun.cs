#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace QHLand
{

    [InitializeOnLoad]
    public class AutosaveOnRun : ScriptableObject
    {
        static AutosaveOnRun()
        {
            EditorApplication.playmodeStateChanged = () =>
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    Debug.Log("Auto-Saving scene before entering Play mode: " + EditorApplication.currentScene);

                    EditorApplication.SaveScene();
                    AssetDatabase.SaveAssets();
                }
            };
        }

        //static AutosaveOnRun()
        //{
        //    EditorApplication.projectWindowChanged = () =>
        //    {
        //        Debug.Log("IN");
        //        EditorApplication.SaveScene();
        //    };
        //}
    }

}


#endif
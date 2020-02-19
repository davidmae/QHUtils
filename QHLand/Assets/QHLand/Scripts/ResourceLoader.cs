using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace QHLand
{

    public static class ResourceLoader
    {
        public static string Path = Paths.EditorResources;
        public static Texture2D ButtomTexturePosition0, ButtomTexturePosition1, ButtomTexturePosition2, ButtomTexturePosition3, ButtomTexturePosition4;
        public static Texture2D ButtomTextureType0, ButtomTextureType1, ButtomTextureType2, ButtomTextureType3;
        public static GUISkin Skin1, Skin2, Skin3, Skin4;

        public static void LoadResources()
        {
            /*ButtomTexturePosition0 = (Texture2D)Resources.LoadAssetAtPath("Assets/Scripts/Editor/Resources/btp0.jpg", typeof(Texture2D));
            ButtomTexturePosition1 = (Texture2D)Resources.LoadAssetAtPath("Assets/Scripts/Editor/Resources/btp1.jpg", typeof(Texture2D));
            ButtomTexturePosition2 = (Texture2D)Resources.LoadAssetAtPath("Assets/Scripts/Editor/Resources/btp2.jpg", typeof(Texture2D));
            ButtomTexturePosition3 = (Texture2D)Resources.LoadAssetAtPath("Assets/Scripts/Editor/Resources/btp3.jpg", typeof(Texture2D));
            ButtomTexturePosition4 = (Texture2D)Resources.LoadAssetAtPath("Assets/Scripts/Editor/Resources/btp4.jpg", typeof(Texture2D));*/

            ButtomTextureType0 = (Texture2D)AssetDatabase.LoadAssetAtPath(Path + "btt0.jpg", typeof(Texture2D));
            ButtomTextureType1 = (Texture2D)AssetDatabase.LoadAssetAtPath(Path + "btt1.jpg", typeof(Texture2D));
            ButtomTextureType2 = (Texture2D)AssetDatabase.LoadAssetAtPath(Path + "btt2.jpg", typeof(Texture2D));
            ButtomTextureType3 = (Texture2D)AssetDatabase.LoadAssetAtPath(Path + "btt3.jpg", typeof(Texture2D));

            Skin1 = (GUISkin)AssetDatabase.LoadAssetAtPath(Path + "Skins/Skin1.guiskin", typeof(GUISkin));
            Skin2 = (GUISkin)AssetDatabase.LoadAssetAtPath(Path + "Skins/Skin2.guiskin", typeof(GUISkin));
            Skin3 = (GUISkin)AssetDatabase.LoadAssetAtPath(Path + "Skins/Skin3.guiskin", typeof(GUISkin));
            Skin4 = (GUISkin)AssetDatabase.LoadAssetAtPath(Path + "Skins/Skin4.guiskin", typeof(GUISkin));
        }


        public static FileInfo[] LoadAllFilesIn(string path)
        {
            var info = new DirectoryInfo(path);
            return info.GetFiles();
        }

    }
}
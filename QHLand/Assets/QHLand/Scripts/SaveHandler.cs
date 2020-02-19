using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace QHLand
{

    public static class SaveHandler
    {
        public static object saveObject = new object();
        public static object deserializedObject = new object();

        //private static string path = Application.persistentDataPath;


        private static byte[] Serialize()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, saveObject);
            return ms.ToArray();
        }

        private static object DeSerialize(byte[] b)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            ms.Write(b, 0, b.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return bf.Deserialize(ms);
        }

        public static void Save(string _path, bool msg)
        {
            byte[] data = Serialize();
            File.WriteAllBytes(_path, data);
            if (msg == false) return;
            Debug.Log("Save in: " + _path);
        }

        public static void Load(string _path, bool msg)
        {
            byte[] b = File.ReadAllBytes(_path);
            deserializedObject = DeSerialize(b);
            if (msg == false) return;
            Debug.Log("Load from: " + _path);
        }

    }
}
using System;
using EditorAttributes;
using UnityEngine;

namespace Systems.FileData.Test
{
    [Serializable]
    public class MyDataTest
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public GameObject prefab;
    }
    public class JsonWriteDataTest : MonoBehaviour
    {
        public JsonData jsonData;
        public Transform objectTransform;
        public MyDataTest myDataTest;
        
        

        [Button("Save")]
        public void SaveJson()
        {
            jsonData.WriteData(myDataTest);    
        }

        [Button("Load")]
        public void LoadJson()
        {
            myDataTest = jsonData.ReadData<MyDataTest>();
        }
    }
}
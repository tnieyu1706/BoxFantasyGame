using EditorAttributes;
using UnityEngine;

namespace Systems.FileData.JsonUtils.JsonDataStatic
{
    public enum LoadOnType
    {
        OnBegin,
        OnEnable
    }

    public enum SaveOnType
    {
        OnDestroy,
        OnDisable
    }

    public class JsonDataStaticSaveLoad : MonoBehaviour
    {
        public JsonDataStatic jsonData;

        #region Options
        
        public bool autoSave = false;
        
        [ShowField(nameof(autoSave))]
        public SaveOnType saveOnType = SaveOnType.OnDestroy;
        
        public bool autoLoad = false;
        
        [ShowField(nameof(autoLoad))]
        public LoadOnType loadOnType = LoadOnType.OnBegin;
        
        #endregion
        
        [Button("Save Data")]
        public void SaveData()
        {
            JsonDataStaticGlobal.Instance.AddOrUpdateData(jsonData);
        }
        
        [Button("Load Data")]
        public void LoadData()
        {
            if (string.IsNullOrEmpty(jsonData?.dataName))
            {
                Debug.Log("dataName is null or empty");
                return;
            }
            
            var dataLoad = JsonDataStaticGlobal.Instance.GetCurrentData(jsonData.dataName);
        
            if (jsonData == null)
            {
                Debug.LogWarning($"Data {jsonData.dataName} is not found");
                return;
            }

            // int i = 0;
            // foreach (var load in dataLoad.components)
            // {
            //     Debug.Log($"old - {jsonData.components[i++].GetInstanceID()} | new - {load.GetInstanceID()}");
            //     foreach (var cur in jsonData.components)
            //     {
            //         if (cur.GetInstanceID() == load.GetInstanceID())
            //         {
            //             ReflectionCopyUtility.ShallowCopy(load, cur);
            //             break;
            //         }
            //     }
            // }

            // jsonData = dataLoad;
            
            Debug.Log($"Loaded completed {jsonData.dataName}");
        }
    }
}
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using JetBrains.Annotations;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace Systems.FileData.JsonUtils.JsonDataStatic
{
    [Serializable]
    public class JsonDataStatic
    {
        public string dataName = "data";

        public List<Component> components;
    }

    [Serializable]
    public class JsonDataStaticGlobalEntity
    {
        [SerializeField, ReadOnly] private SerializedDictionary<string, JsonDataStatic> datas = new();

        public SerializedDictionary<string, JsonDataStatic> Datas => datas;
    }

    /// <summary>
    /// Json data static is a global data for all json data static will be save load.
    /// Have 2 levels save load:
    /// Level1: SO <-> Object.
    /// Level2: File <-> SO.
    /// </summary>
    [CreateAssetMenu(fileName = "JsonDataStaticGlobal",
        menuName = "Scriptable Objects/FileData/JsonSupport/JsonDataStaticGlobal")]
    public class JsonDataStaticGlobal : SingletonScriptable<JsonDataStaticGlobal>
    {
        [Required] public JsonData jsonFile;

        public JsonDataStaticGlobalEntity entity = new();

        /// <summary>
        /// Save data Level 1.
        /// Save data to this StaticGlobalData.
        /// </summary>
        /// <param name="data"></param>
        public void AddOrUpdateData(JsonDataStatic data)
        {
            entity.Datas[data.dataName] = data;
        }

        /// <summary>
        /// Load data Level 1.
        /// Load data from staticGlobalData.
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns></returns>
        [CanBeNull]
        public JsonDataStatic GetCurrentData(string dataName)
        {
            if (entity.Datas.TryGetValue(dataName, out JsonDataStatic data))
            {
                return data;
            }

            Debug.Log($"Data {dataName} not found.");
            return null;
        }

        /// <summary>
        /// Save data Level2.
        /// Save data to File. Save persistent data.
        /// </summary>
        [Button("Save All Data To File Manually")]
        public void SaveAllDataToFile()
        {
            if (jsonFile == null)
            {
                Debug.LogError("JsonDataStaticGlobal is null.");
                return;
            }

            jsonFile.WriteData(entity);
        }

        /// <summary>
        /// Load data Level2.
        /// Load dt from file. Reload full persistent data.
        /// </summary>
        [Button("Load All Data To File Manually")]
        public void LoadAllDataToFile()
        {
            if (jsonFile == null)
            {
                Debug.LogError("JsonDataStaticGlobal is null.");
                return;
            }

            entity = jsonFile.ReadData<JsonDataStaticGlobalEntity>();
        }
    }
}
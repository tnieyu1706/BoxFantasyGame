using EditorAttributes;
using UnityEngine;

namespace Systems.FileData
{
    [CreateAssetMenu(fileName = "JsonData", menuName = "Scriptable Objects/FileData/JsonData")]
    public class JsonData : FileData<JsonService>
    {
        [SerializeField, FilePath(true, "json")] private string filePath;
        
        public override string Path => filePath;

        public void WriteData<T>(T data)
        {
            if (Path == string.Empty)
            {
                Debug.LogWarning("Path is empty.");
                return;
            }
            
            service.WriteData(Path, data);
            Debug.Log($"JsonData-{name} has been written to {filePath}");
        }

        public T ReadData<T>()
        {
            if (Path == string.Empty)
            {
                Debug.LogWarning("Path is empty.");
                return default(T);
            }
            var data = service.ReadData<T>(Path);

            Debug.Log($"Have just read data from JsonData-{name}");
            return data;
        }
    }
}
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;

namespace Systems.FileData
{
    [CreateAssetMenu(fileName = "CsvData", menuName = "Scriptable Objects/FileData/CsvData")]
    public class CsvData : FileData<CsvService>
    {
        [SerializeField, FilePath(true, "csv")]
        private string filePath;
        
        public override string Path => filePath;

        public void WriteData<T>(IEnumerable<T> datas)
        {
            if (Path == string.Empty)
            {
                Debug.LogWarning("Path is empty.");
                return;
            }
            
            service.WriteData(Path, datas);
            Debug.Log($"Data written to {Path}");
        }

        public IEnumerable<T> ReadData<T>()
        {
            if (Path == string.Empty)
            {
                Debug.LogWarning("Path is empty.");
                return null;
            }
            
            var datas = service.ReadData<T>(filePath);
            Debug.Log($"Data read from {Path}");
            return datas;
        }
    }
}
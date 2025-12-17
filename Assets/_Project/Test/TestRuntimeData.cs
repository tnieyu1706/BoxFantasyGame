using EditorAttributes;
using Newtonsoft.Json.Linq;
using TnieYuPackage.SaveLoadSystem.RuntimeSaveLoad;
using TnieYuPackage.Utils;
using UnityEngine;

namespace _Project.Test
{
    public class TestRuntimeData : MonoBehaviour, IRuntimeData
    {
        [SerializeField, ReadOnly] private SerializableGuid runtimeGuid = SerializableGuid.NewGuid();
        
        public string value1;
        public string value2;
        public float floatValue;

        public SerializableGuid RuntimeId
        {
            get => runtimeGuid;
            set => runtimeGuid = value;
        }

        public string Save()
        {
            return JsonUtility.ToJson(this, true);
        }

        public void Load(string jsonData)
        {
            JsonUtility.FromJsonOverwrite(jsonData, this);
        }
    }
}
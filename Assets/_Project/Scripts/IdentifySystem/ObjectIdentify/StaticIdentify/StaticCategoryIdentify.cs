using System.Collections.Generic;
using System.Linq;
using TnieYuPackage.Utils;
using UnityEngine;

namespace Systems.IdentifySystem.ObjectIdentify.StaticIdentify
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Identify/Static Identify/Category")]
    public class StaticCategoryIdentify : ScriptableObject, ICategoryIdentify<StaticObjectIdentify>
    {
        [SerializeField] private string categoryName;
        [SerializeField]
        private List<StaticObjectIdentify> objectIdentifies = new List<StaticObjectIdentify>();
        
        //properties
        public string CategoryName => categoryName;
        public List<StaticObjectIdentify> ObjectIdentifies => objectIdentifies;
        
        public Dictionary<SerializableGuid, StaticObjectIdentify> GetMapById()
        {
            return ObjectIdentifies.ToDictionary(x => x.Id);
        }

        public Dictionary<string, StaticObjectIdentify> GetMapByName()
        {
            return ObjectIdentifies.ToDictionary(x => x.ObjectName);
        }
    }
}
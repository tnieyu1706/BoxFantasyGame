using TnieYuPackage.Utils;
using UnityEngine;

namespace Systems.IdentifySystem.ObjectIdentify.StaticIdentify
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Identify/Static Identify/Manager")]
    public class StaticCategoryIdentifyManager : 
        BaseCategoryIdentifyManager<
            StaticCategoryIdentify, 
            StaticObjectIdentify,
            StaticCategoryIdentifyManager>
    {
        public override StaticObjectIdentify QuickGetObjectById(string categoryIdentify, SerializableGuid objectId)
        {
            this.GetCategoryByCategoryIdentify(categoryIdentify)
                .GetMapById()
                .TryGetValue(objectId, out StaticObjectIdentify result);
            return result;
        }

        public override StaticObjectIdentify QuickGetObjectByName(string categoryIdentify, string objectName)
        {
            this.GetCategoryByCategoryIdentify(categoryIdentify)
                .GetMapByName()
                .TryGetValue(objectName, out StaticObjectIdentify result);
            
            return result;
        }
    }
}
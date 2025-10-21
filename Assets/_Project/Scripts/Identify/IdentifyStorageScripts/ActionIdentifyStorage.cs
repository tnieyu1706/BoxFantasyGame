using UnityEngine;

namespace Systems.Identify
{
    public class ActionIdentifyStorageFlagAttribute : TypeIdentifyStorageFlagAttribute
    {
        public new const string StorageIdentify = "GlobalAction";
        
        public ActionIdentifyStorageFlagAttribute(string identify) : base(identify)
        {
            
        }
    }
    
    [CreateAssetMenu(fileName = "ActionIdentifyStorage", menuName = "Scriptable Objects/IdentifyStorage/ActionIdentifyStorage")]
    public class ActionIdentifyStorage : TypeIdentifyStorage<ActionIdentifyStorageFlagAttribute, ActionIdentifyStorage>
    {
    }
}
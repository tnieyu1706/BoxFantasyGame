using UnityEngine;

namespace Systems.Identify
{
    public class SubjectIdentifyStorageFlagAttribute : TypeIdentifyStorageFlagAttribute
    {
        public new const string StorageIdentify = "GlobalSubject";
        
        public SubjectIdentifyStorageFlagAttribute(string identify) : base(identify)
        {
            
        }
    }
    
    [CreateAssetMenu(fileName = "SubjectIdentifyStorage", menuName = "Scriptable Objects/IdentifyStorage/SubjectIdentifyStorage")]
    public class SubjectIdentifyStorage : TypeIdentifyStorage<SubjectIdentifyStorageFlagAttribute, SubjectIdentifyStorage>
    {
    }
}
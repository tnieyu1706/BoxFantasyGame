using EditorAttributes;
using TnieYuPackage.Utils;
using UnityEngine;

namespace Systems.IdentifySystem.ObjectIdentify.StaticIdentify
{
    public abstract class StaticObjectIdentify : ScriptableObject, IObjectIdentify<StaticCategoryIdentify>
    {
        #region FIELDS

        [SerializeField, ReadOnly] private SerializableGuid id = SerializableGuid.NewGuid();
        [SerializeField] private string objectName;
        [SerializeField] private StaticCategoryIdentify staticCategoryIdentify; 

        #endregion
        
        #region PROPERTIES
        // properties
        public SerializableGuid Id => this.id;
        public string ObjectName => objectName;
        public StaticCategoryIdentify CategoryIdentify => staticCategoryIdentify;
        
        #endregion
        
        #region BUTTONS

        [Button("Registry")]
        private void RegistryObjectIdentify()
        {
            if (string.IsNullOrEmpty(objectName) || staticCategoryIdentify == null)
            {
                Debug.LogWarning("objectName and staticCategoryIdentify are null!");
                return;
            }

            if (CategoryIdentify.GetMapById().ContainsKey(this.Id))
            {
                Debug.Log("Have registry this object");
                return;
            }
            
            CategoryIdentify.ObjectIdentifies.Add(this);
        }
        
        #endregion
    }
}
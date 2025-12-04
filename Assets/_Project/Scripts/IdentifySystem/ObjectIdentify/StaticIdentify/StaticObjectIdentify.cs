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

    public abstract class StaticObjectSingletonIdentify<T> : StaticObjectIdentify
        where T : StaticObjectSingletonIdentify<T>
    {
        #region Singleton

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
#if UNITY_EDITOR
                    // Trong Editor thì tìm bằng AssetDatabase cho tiện
                    string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
                    if (guids.Length == 0)
                    {
                        Debug.LogError($"No assets found in '{typeof(T).Name}'!");
                        return null;
                    }

                    if (guids.Length > 0)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                    }
#else
                // Runtime thì load từ Resources
                instance = Resources.Load<T>(typeof(T).Name);
                if (instance == null) {
                    Debug.LogError($"No assets found in '{typeof(T).Name}'!");
                }
#endif
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                // Không nên xóa, chỉ cảnh báo thôi
                Debug.LogWarning($"Duplicate {typeof(T).Name} detected: {name}");
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using AYellowpaper;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace Systems.Identify
{
    [CreateAssetMenu(fileName = "TypeIdentifyStorageManager", menuName = "Scriptable Objects/IdentifyStorage/_IdentifyStorageManager")]
    public class TypeIdentifyStorageManager : SingletonScriptable<TypeIdentifyStorageManager>
    {
        public List<InterfaceReference<ITypeIdentifyStorage, ScriptableObject>> IdentifyStorages = new();

        [Space(20)]
        public Void spacing;
        
        [Button("Update All Identify Storage")]
        public void UpdateAllIdentifyStorage()
        {
            if (IdentifyStorages == null || IdentifyStorages.Count == 0)
            {
                Debug.LogWarning("No identify storage found!");
                return;
            }
            
            IdentifyStorages.ForEach(i => i.Value?.UpdateIdentifyDataStorage());
        }
    }
}
// using AYellowpaper;
// using EditorAttributes;
// using TnieYuPackage.DesignPatterns.Patterns.Singleton;
// using TnieYuPackage.Utils.DictionaryUtil;
// using UnityEngine;
// using Void = EditorAttributes.Void;
//
// namespace Systems.IdentifySystem.TypeIdentify
// {
//     [CreateAssetMenu(fileName = "TypeIdentifyStorageManager",
//         menuName = "Scriptable Objects/IdentifyStorage/_IdentifyStorageManager")]
//     public class TypeIdentifyStorageManager : SingletonScriptable<TypeIdentifyStorageManager>
//     {
//         public SerializableDictionary<string, InterfaceReferenceGUI<ITypeIdentifyStorage, ScriptableObject>>
//             IdentifyStorages = new();
//
//         [Space(20)] public Void spacing;
//
//         // private readonly string assemblyName = "Assembly-CSharp";
//
//         // [Button("Refresh Type Storages")]
//         // public void RefreshTypeIdentifyStorages()
//         // {
//         //     Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
//         //         .FirstOrDefault(assembly => assembly.GetName().Name == assemblyName);
//         //
//         //     if (assembly == null)
//         //     {
//         //         Debug.LogError(assemblyName + " could not be found.");
//         //         return;
//         //     }
//         //
//         //     var storageTypes = assembly.GetTypes()
//         //         .Where(t => typeof(ScriptableObject).IsAssignableFrom(t) &&
//         //                     typeof(ITypeIdentifyStorage).IsAssignableFrom(t) &&
//         //                     !t.IsAbstract &&
//         //                     t.BaseType != null)
//         //         .ToList();
//         //
//         //     // Type baseType;
//         //     // Type attributeType;
//         //     // FieldInfo storageIdentifyField;
//         //     string keyName = null;
//         //
//         //     PropertyInfo instanceProperty;
//         //     object instanceObject;
//         //     ITypeIdentifyStorage idStorage = null;
//         //
//         //     IdentifyStorages.data.Clear();
//         //
//         //     foreach (var type in storageTypes)
//         //     {
//         //         Debug.Log("type name: " + type.FullName);
//         //
//         //         var singletonType = typeof(SingletonScriptable<>).MakeGenericType(type);
//         //         instanceProperty = singletonType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
//         //
//         //         if (instanceProperty == null)
//         //         {
//         //             Debug.LogError("Instance property could not be found.");
//         //             continue;
//         //         }
//         //
//         //         instanceObject = instanceProperty.GetValue(null);
//         //
//         //         if (instanceObject is ITypeIdentifyStorage storage)
//         //         {
//         //             keyName = storage.GetStorageIdentify();
//         //             idStorage = storage;
//         //         }
//         //
//         //         //final check
//         //         if (keyName != null && idStorage != null)
//         //         {
//         //             IdentifyStorages.AddOrUpdate(keyName,
//         //                 new InterfaceReferenceGUI<ITypeIdentifyStorage, ScriptableObject>(idStorage));
//         //         }
//         //     }
//         // }
//
//         [Button("Update All Identify Storage")]
//         public void UpdateAllIdentifyStorage()
//         {
//             if (IdentifyStorages == null || IdentifyStorages.data.Count == 0)
//             {
//                 Debug.LogWarning("No identify storage found!");
//                 return;
//             }
//
//             foreach (var storage in IdentifyStorages.Dictionary)
//             {
//                 storage.Value?.Value?.UpdateIdentifyDataStorage();
//             }
//         }
//
//         [Button("Clear All Identify Storage")]
//         public void ClearAllIdentifyStorage()
//         {
//             IdentifyStorages.data.Clear();
//         }
//     }
// }
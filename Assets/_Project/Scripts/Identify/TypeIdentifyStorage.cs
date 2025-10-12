using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Identify
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class TypeIdentifyStorageFlagAttribute : Attribute
    {
        public string Identify;

        public TypeIdentifyStorageFlagAttribute(string identify)
        {
            this.Identify = identify;
        }
    }

    public abstract class TypeIdentifyStorage<TAttribute, TSingleton> : SingletonScriptable<TSingleton>
        where TSingleton : ScriptableObject
        where TAttribute : TypeIdentifyStorageFlagAttribute
    {
        [SerializedDictionary("Identify, Type")]
        [ReadOnly]
        public SerializedDictionary<string, string> datas = new();

        private bool ValidateKey(string key)
        {
            if (datas.ContainsKey(key))
                return false;

            return true;
        }

        private bool ValidateType(Type typeValidated)
        {
            return typeValidated?.GetAttribute<TAttribute>() != null;
        }

        // public abstract TypeIdentifyStorage<TAttribute, TSingleton> GetStorage();
        //
        // public abstract string TypeIdentifyStorageName();
        //
        // public void RegistryIdentify(string identify, Type type)
        // {
        //     if (GetStorage() == null)
        //     {
        //         Debug.LogError($"{TypeIdentifyStorageName()} not yet instantiated Scriptable Object Data.");
        //         
        //     }
        //     if (ValidateKey(identify) && ValidateType(type))
        //     {
        //         GetStorage().datas.Add(identify, type.AssemblyQualifiedName);
        //     }
        // }

        [Button("Update Identify Data Storage Manual")]
        private void UpdateIdentifyDataStorage()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var validTypes = assembly.GetTypes()
                .Where(ValidateType)
                .ToList();

            datas.Clear();

            foreach (var type in validTypes)
            {
                var attr = type.GetAttribute<TAttribute>();
                if (attr != null)
                {
                    if (!ValidateKey(attr.Identify))
                    {
                        Debug.LogWarning($"your key {attr.Identify} is not valid! - by type {type.AssemblyQualifiedName}");
                        continue;
                    }

                    if (!ValidateType(type))
                    {
                        Debug.LogWarning($"type {type.AssemblyQualifiedName} is not valid!");
                        continue;
                    }

                    datas.Add(attr.Identify, type.AssemblyQualifiedName);
                }
            }
        }
    }
}
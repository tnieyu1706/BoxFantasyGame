using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EditorAttributes;
using JetBrains.Annotations;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using UnityEngine.Serialization;
using Void = EditorAttributes.Void;

namespace Systems.IdentifySystem.TypeIdentify
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class TypeIdentifyStorageFlagAttribute : Attribute
    {
        public string Identify;
        public const string StorageIdentify = "TypeGlobal";

        public TypeIdentifyStorageFlagAttribute(string identify)
        {
            this.Identify = identify;
        }

        /// <summary>
        /// Get StorageIdentify from this (FlagAttribute object)
        /// </summary>
        /// <returns></returns>
        public string GetActualStorageIdentify()
        {
            var storageIdField = this.GetType()
                .GetField(
                    nameof(StorageIdentify),
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic
                );
            
            if (storageIdField == null)
            {
                Debug.LogWarning(
                    $"{this.GetType().FullName} don't found {nameof(TypeIdentifyStorageFlagAttribute.StorageIdentify)} field");
                return string.Empty;
            }

            return storageIdField.GetValue(null) as string;
        }
    }

    public interface ITypeIdentifyStorage
    {
        void UpdateIdentifyDataStorage();

        string GetStorageIdentify();
    }

    public abstract class TypeIdentifyStorage<TAttribute, TSingleton> : SingletonScriptable<TSingleton>,
        ITypeIdentifyStorage
        where TAttribute : TypeIdentifyStorageFlagAttribute
        where TSingleton : ScriptableObject
    {
        [ReadOnly]
        public string storageIdentify;

        public string assemblyName;

        [SerializeField, ReadOnly]
        private SerializableDictionary<string, string> datas = new();

        public SerializableDictionary<string, string> Datas => datas;

        public Dictionary<string, Type> MapByTypes()
        {
            return datas.Dictionary.ToDictionary(kvp => kvp.Key, kvp => Type.GetType(kvp.Value));
        }

        /// <summary>
        /// Get StorageIdentify depend on TAttribute.
        /// </summary>
        /// <returns></returns>
        public string GetStorageIdentify()
        {
            var identifyStorageField = typeof(TAttribute).GetField(
                nameof(TypeIdentifyStorageFlagAttribute.StorageIdentify),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (identifyStorageField == null)
                return string.Empty;

            return identifyStorageField.GetValue(null) as string;
        }

        private bool ValidateKey(string key)
        {
            if (Datas.Dictionary.ContainsKey(key))
                return false;

            return true;
        }

        private bool ValidateType(Type typeValidated)
        {
            return typeValidated?.GetCustomAttribute<TAttribute>() != null;
        }

        public string GetIdByType(string assemblyTypeName)
        {
            foreach (var kvp in Datas.Dictionary)
            {
                if (kvp.Value == assemblyTypeName)
                {
                    return kvp.Key;
                }
            }
            
            return string.Empty;
        }

        public string GetIdByType(Type type)
        {
            string assemblyTypeName = type.AssemblyQualifiedName;
            return GetIdByType(assemblyTypeName);
        }

        [CanBeNull]
        public Type GetType(string key)
        {
            if (Datas.Dictionary.TryGetValue(key, out var result))
            {
                return Type.GetType(result);
            }

            return null;
        }

        [Space(20)] public Void spacing;

        [ButtonField(nameof(UpdateIdentifyDataStorage))]
        public Void updateIdentifyButton;

        public void UpdateIdentifyDataStorage()
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                Debug.LogError("AssemblyName is empty");
                return;
            }
            storageIdentify = GetStorageIdentify();
            var assembly = Assembly.Load(assemblyName);

            if (assembly == null)
            {
                Debug.LogError($"can't load assembly {assemblyName}");
                return;
            }

            var validTypes = assembly.GetTypes()
                .Where(ValidateType)
                .ToList();

            Datas.data.Clear();

            foreach (var type in validTypes)
            {
                var attr = type.GetCustomAttribute<TAttribute>();
                if (attr != null)
                {
                    if (!ValidateKey(attr.Identify))
                    {
                        Debug.LogWarning(
                            $"your key {attr.Identify} is not valid! - by type {type.AssemblyQualifiedName}");
                        continue;
                    }

                    if (!ValidateType(type))
                    {
                        Debug.LogWarning($"type {type.AssemblyQualifiedName} is not valid!");
                        continue;
                    }

                    Datas.AddOrUpdate(attr.Identify, type.AssemblyQualifiedName);
                }
            }
        }
    }
}
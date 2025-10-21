using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using Unity.VisualScripting;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace Systems.Identify
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

        [SerializeField, SerializedDictionary("Identify, Type")] [EditorAttributes.ReadOnly]
        private SerializedDictionary<string, string> datas = new();

        public SerializedDictionary<string, string> Datas => datas;

        protected override void Awake()
        {
            base.Awake();
            storageIdentify = GetStorageIdentify();
        }

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
            if (Datas.ContainsKey(key))
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

        [Space(20)] public Void spacing;

        [ButtonField(nameof(UpdateIdentifyDataStorage))]
        public Void upateIdentifyButton;

        public void UpdateIdentifyDataStorage()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var validTypes = assembly.GetTypes()
                .Where(ValidateType)
                .ToList();

            Datas.Clear();

            foreach (var type in validTypes)
            {
                var attr = type.GetAttribute<TAttribute>();
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

                    Datas.Add(attr.Identify, type.AssemblyQualifiedName);
                }
            }
        }
    }
}
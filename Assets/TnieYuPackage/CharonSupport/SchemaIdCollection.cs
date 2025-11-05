using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using TnieYuPackage.GlobalExtensions;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;

namespace TnieYuPackage.CharonSupport
{
    [CreateAssetMenu(fileName = "SchemaIdCollection",
        menuName = "TnieYuPackage/CharonSupport/SchemaIdCollection")]
    public class SchemaIdCollection : SingletonScriptable<SchemaIdCollection>
    {
        public string charonNamespace;

        private string[] excludedSchemas =
        {
            "Assets._Project.Charon.Formulas",
            "Assets._Project.Charon.Formatters"
        };

        public SerializableDictionary<string, List<string>> collections = new();

        [Button("Refresh Schema Collections")]
        public void LoadAllSchemaCollection()
        {
            if (charonNamespace == null)
            {
                throw new System.ArgumentNullException($"charonNamespace is null.");
            }

            var schemaIdTypes = GetStaticIdClasses(charonNamespace, excludedSchemas);

            collections.data.Clear();
            foreach (var schemaIdType in schemaIdTypes)
            {
                LoadCollectionType(schemaIdType);
            }

            Debug.Log("Loaded all schema collections.");
        }

        public void LoadCollectionType(Type type)
        {
            if (type == null)
            {
                Debug.LogWarning("Invalid schema type: " + type.GetShortAssemblyName());
                return;
            }

            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var schemaIds = GetOrAddCollection(type);

            schemaIds.Clear();
            foreach (var field in fields)
            {
                schemaIds.Add(field.GetValue(null) as string);
            }

            Debug.Log($"Load schema data for {type.Name} completed.");
        }

        private List<string> GetOrAddCollection(Type type)
        {
            if (type == null)
            {
                Debug.Log("type is empty.");
                return null;
            }

            string shortAssemblyName = type.GetShortAssemblyName();
            if (!collections.Dictionary.ContainsKey(shortAssemblyName))
            {
                collections.AddOrUpdate(shortAssemblyName, new List<string>());
            }

            return collections[shortAssemblyName];
        }

        public List<string> GetCollection(Type type)
        {
            if (type == null)
            {
                Debug.Log("type is empty.");
                return null;
            }

            string shortAssemblyName = type.GetShortAssemblyName();
            if (!collections.Dictionary.ContainsKey(shortAssemblyName))
            {
                Debug.Log($"{shortAssemblyName} is not valid or not found.");
                return null;
            }

            return collections[shortAssemblyName];
        }
        
        public static Type[] GetStaticIdClasses(string targetNamespace, params string[] excludeNamespaces)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => 
                    t is { IsClass: true, Namespace: not null } 
                    && t.IsAbstract && t.IsSealed // static class
                    && t.Name.EndsWith("Id", StringComparison.Ordinal) // kết thúc bằng "Id"
                    && t.Namespace.StartsWith(targetNamespace)
                    && !excludeNamespaces.Any(ex => t.Namespace.StartsWith(ex))
                )
                .ToArray();
        }

        // Dự phòng cho các assembly có thể bị lỗi load type (ví dụ UnityEditor)
        private static Type[] SafeGetTypes(Assembly assembly)
        {
            try { return assembly.GetTypes(); }
            catch { return Array.Empty<Type>(); }
        }
    }
}
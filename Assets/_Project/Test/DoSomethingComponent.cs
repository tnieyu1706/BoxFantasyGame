using System;
using EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace _Project.Test
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class TestTypeCacheAttribute : Attribute
    {
        
    }
    
    public class DoSomethingComponent : MonoBehaviour
    {
        [TestTypeCache]
        public string testValue;
        
        [Button]
        private void TestLogTypeCache()
        {
            var fields = TypeCache.GetFieldsWithAttribute<TestTypeCacheAttribute>();

            foreach (var f in fields)
            {
                Debug.Log($"{f.Name} - {f.GetValue(this)}");
            }
        } 
    }
}
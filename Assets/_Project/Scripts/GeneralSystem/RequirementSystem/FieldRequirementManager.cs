using System;
using EditorAttributes;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using TnieYuPackage.ObjectDataSystem;
using TnieYuPackage.Utils;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;

namespace Systems.GeneralSystem.RequirementSystem
{
    [CreateAssetMenu(fileName = "FieldRequirementManager", menuName = "Scriptable Objects/Quest/FieldManager")]
    public class FieldRequirementManager : SingletonScriptable<FieldRequirementManager>
    {
        public SerializableDictionary<string, FieldRequirementValue> fields;

        public bool Validate(string key)
        {
            return fields.Dictionary.ContainsKey(key);
        }
        
        public bool TryToGetField(string key, out FieldRequirementValue fieldRequirementValue)
        {
            if (Validate(key))
            {
                fieldRequirementValue = fields.Dictionary[key];
                return true;
            }

            fieldRequirementValue = null;
            return false;
        }
        
        //test
        
        // [Button("TestCallValueLoadEvent")]
        // private void CallOnValueLoad(string key)
        // {
        //     if (fields.Dictionary.TryGetValue(key, out var value))
        //     {
        //         value.OnValueLoad?.Invoke(value.Value);
        //         Debug.Log("Call OnValueLoad");
        //     }
        // }
    }

    [Serializable]
    public class FieldRequirementValue
    {
        [SerializeReference, AbstractSupport(typeof(IObjectData))]
        private IObjectData value;

        public IObjectData Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueLoad?.Invoke(this.value);
            }
        }

        public SafeTriggerAction<IObjectData> OnValueLoad = new();
    }
}
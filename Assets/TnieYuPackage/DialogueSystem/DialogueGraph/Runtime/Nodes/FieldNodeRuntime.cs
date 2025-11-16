using System;
using JetBrains.Annotations;
using Systems.GeneralSystem.ObjectDataSystem;
using TnieYuPackage.Utils;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class FieldNodeRuntime : NodeRuntime
    {
        public FieldType fieldType;
        [SerializeReference]
        public ObjectData data;

        public FieldNodeRuntime(SerializableGuid id, FieldType fieldType, object value) : base(id)
        {
            this.fieldType = fieldType;
            
            ObjectData.SetValueDirectly(this.fieldType, value, out data);
        }
    }

    public static class FieldDataSupport
    {
        [CanBeNull]
        public static FieldNodeRuntime GetFieldNodeRuntimeById(SerializableGuid fieldId)
        {
            //Load fieldNode from graph runtime.
            var graph = DialogueGraphRuntime.CurrentGraphRuntime;
            if (graph == null)
            {
                Debug.LogWarning("Current Graph runtime is null");
                return null;
            }

            graph.FieldsDict.TryGetValue(fieldId, out var field);

            if (field == null)
            {
                Debug.Log($"Not found id {fieldId} in current graph runtime");
                return null;
            }

            return field;
        }
    }
    
}
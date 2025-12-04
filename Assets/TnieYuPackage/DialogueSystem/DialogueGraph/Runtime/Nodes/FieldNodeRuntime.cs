using System;
using JetBrains.Annotations;
using TnieYuPackage.ObjectDataSystem;
using TnieYuPackage.Utils;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class FieldNodeRuntime : NodeRuntime
    {
        public FieldType fieldType;
        [SerializeReference]
        public IObjectData data;

        public FieldNodeRuntime(SerializableGuid id, FieldType fieldType, object value) : base(id)
        {
            this.fieldType = fieldType;

            data = FieldTypeSupport.FieldTypes[fieldType].ConvertToObjectDataProcedure.Invoke();
            data.Value = value;
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
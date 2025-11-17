using System;
using Systems.GeneralSystem.ObjectDataSystem;
using TnieYuPackage.Utils;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    /// <summary>
    /// Only set value with FieldNode.
    /// </summary>
    [Serializable]
    public class SetFieldNodeRuntime : FlowNodeRuntime<SetFieldNodeRuntime>
    {
        public SerializableGuid fieldId = SerializableGuid.Empty;
        public FieldType type;
        [SerializeReference] public ObjectData setData;

        public SetFieldNodeRuntime(SerializableGuid id, FieldType type, object setValue) : base(id)
        {
            this.type = type;

            ObjectData.SetValueDirectly(this.type, setValue, out setData);
        }

        public override void Execute()
        {
            if (fieldId == SerializableGuid.Empty)
            {
                Debug.Log("Current field id is empty");
                return;
            }

            var field = FieldDataSupport.GetFieldNodeRuntimeById(fieldId);

            if (field != null && field.fieldType == type && ObjectData.ValidateData(setData, field.data.Value))
            {
                field.data.Value = setData.Value;
            }
        }
    }
}
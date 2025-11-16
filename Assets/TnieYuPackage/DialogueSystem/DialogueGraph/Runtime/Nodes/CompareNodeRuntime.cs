using System;
using Systems.GeneralSystem.ObjectDataSystem;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Generals;
using TnieYuPackage.Utils;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class CompareNodeRuntime : FlowNodeRuntime<CompareNodeRuntime>
    {
        public FieldType compareType;
        public SerializableGuid aFieldId = SerializableGuid.Empty;
        public SerializableGuid bFieldId = SerializableGuid.Empty;
        public CompareOperator compareOperator;

        public SerializableGuid trueNextId = SerializableGuid.Empty;
        public SerializableGuid falseNextId = SerializableGuid.Empty;

        public CompareNodeRuntime(
            SerializableGuid id, 
            FieldType compareType, 
            CompareOperator compareOperator) : base(id)
        {
            this.compareType = compareType;
            this.compareOperator = compareOperator;
        }

        private CompareOperatorData CompareOperatorData => CompareOperatorSupport.Operators[compareOperator];
        
        public override void Execute()
        {
            //check nextId;
            if (trueNextId == SerializableGuid.Empty || falseNextId == SerializableGuid.Empty)
            {
                Debug.Log("trueNextId & falseNextId is not set");
                return;
            }
            
            //Load aField & bFieldId;
            if (aFieldId == SerializableGuid.Empty || bFieldId == SerializableGuid.Empty)
            {
                Debug.Log("aFieldId and bFieldId is null");
                return;
            }
            
            var aField = FieldDataSupport.GetFieldNodeRuntimeById(aFieldId);
            var bField = FieldDataSupport.GetFieldNodeRuntimeById(bFieldId);

            if (aField == null || bField == null)
            {
                Debug.Log("aFieldId and bFieldId is null");
                return;
            }

            //Validate actual value and set nextId
            if (ObjectData.ValidateData(aField.data, bField.data) &&
                aField.data.Value is IComparable aValue &&
                bField.data.Value is IComparable bValue)
            {
                bool resultCompare = CompareOperatorData.Compare.Invoke(aValue, bValue);
                NextId = resultCompare ? trueNextId : falseNextId;
            }
        }
    }
}
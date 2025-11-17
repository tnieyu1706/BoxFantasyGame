using System;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class SetFieldNode : ActionNode, IFieldTypeDefinition
    {
        public const string FIELD_VALUE_SET_NAME = "ValueSet";
        public const string FIELD_IDENTITY_NAME = "Field";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            this.DefineFieldTypeOptions(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
            
            this.DefineFieldTypePorts(context);
        }

        public Node GetNode => this;
        public void FieldTypeSetupPorts<T>(IPortDefinitionContext ctx)
        {
            ctx.AddInputPort<T>(FIELD_VALUE_SET_NAME)
                .Build();

            ctx.AddInputPort<T>(FIELD_IDENTITY_NAME)
                .Build();
        }
    }
}
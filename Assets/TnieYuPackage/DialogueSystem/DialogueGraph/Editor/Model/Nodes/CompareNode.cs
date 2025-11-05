using System;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Generals;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class CompareNode : BaseNode, IFieldTypeDefinition
    {
        public const string TRUE_EXECUTION_PORT_NAME = "True";
        public const string FALSE_EXECUTION_PORT_NAME = "False";
        
        public const string COMPARE_OPERATOR_NAME = "Operator";
        public const string A_FIELD_NAME = "AField";
        public const string B_FIELD_NAME = "BField";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            this.DefineFieldTypeOptions(context);

            context.AddOption<CompareOperator>(COMPARE_OPERATOR_NAME)
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            context.AddOutputPort(TRUE_EXECUTION_PORT_NAME)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            context.AddOutputPort(FALSE_EXECUTION_PORT_NAME)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            this.DefineFieldTypePorts(context);
        }

        public Node GetNode => this;
        public void FieldTypeSetupPorts<T>(IPortDefinitionContext ctx)
        {
            ctx.AddInputPort<T>(A_FIELD_NAME)
                .Build();

            ctx.AddInputPort<T>(B_FIELD_NAME)
                .Build();
        }
    }
}
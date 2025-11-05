using System;
using TnieYuPackage.GTKExtensions;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal abstract class FlowNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
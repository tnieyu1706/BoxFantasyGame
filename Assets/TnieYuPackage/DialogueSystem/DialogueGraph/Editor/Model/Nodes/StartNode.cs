using System;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    /// <summary>
    /// Node to specify first node in graph & Configuration for graph.
    /// </summary>
    [Serializable]
    internal class StartNode : Node
    {
        public const string IS_BACKUP_NAME = "IsBackup";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<bool>(IS_BACKUP_NAME)
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(BaseNode.EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
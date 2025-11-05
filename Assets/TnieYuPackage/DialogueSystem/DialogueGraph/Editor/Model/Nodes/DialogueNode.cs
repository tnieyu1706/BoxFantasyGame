using System;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal abstract class BaseDialogueNode : BaseNode
    {
        public const string CHARACTER_FIELD_NAME = "Character";
        public const string CONTENT_FIELD_NAME = "Content";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(String.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
                
            //field
            context.AddInputPort<string>(CHARACTER_FIELD_NAME)
                .WithDisplayName(CHARACTER_FIELD_NAME)
                .Build();
            context.AddInputPort<string>(CONTENT_FIELD_NAME)
                .WithDisplayName(CONTENT_FIELD_NAME)
                .Build();
        }
    }
    [Serializable]
    internal class DialogueNode : BaseDialogueNode
    {
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
            
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(String.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
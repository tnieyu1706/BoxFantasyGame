using System;
using TnieYuPackage.GTKExtensions;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class MethodNode : FlowNode
    {
        public const string METHOD_PORT_NAME = "Method";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            context.AddInputPort<string>(METHOD_PORT_NAME)
                .Build();
        }
    }
}
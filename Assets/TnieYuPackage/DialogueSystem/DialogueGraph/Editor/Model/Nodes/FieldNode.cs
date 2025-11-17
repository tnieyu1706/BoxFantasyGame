using System;
using TnieYuPackage.GTKExtensions;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class FieldNode : CheckedNode, IFieldTypeDefinition
    {
        public const string VALUE_FIELD_INPUT_NAME = "Def";
        public const string VALUE_FIELD_OUTPUT_NAME = "Out";

        public const string FIELD_NAME = "Name";

        /// <summary>
        /// setup default value for fieldValue manual by importer.
        /// </summary>
        public object fieldValue;

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<string>(FIELD_NAME)
                .Build();
                
            this.DefineFieldTypeOptions(context);
            
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            this.DefineFieldTypePorts(context);
        }

        public Node GetNode => this;

        public void FieldTypeSetupPorts<T>(IPortDefinitionContext ctx)
        {
            ctx.AddInputPort<T>(VALUE_FIELD_INPUT_NAME)
                .Build();
            ctx.AddOutputPort<T>(VALUE_FIELD_OUTPUT_NAME)
                .Build();
        }
    }
}
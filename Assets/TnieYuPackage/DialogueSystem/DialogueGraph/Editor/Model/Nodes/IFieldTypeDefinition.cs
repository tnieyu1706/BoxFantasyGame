using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Generals;
using TnieYuPackage.GTKExtensions;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    /// <summary>
    /// Interface mixin use to provide FieldType (option & port) => context.
    /// </summary>
    internal interface IFieldTypeDefinition
    {
        public const string FIELD_TYPE_NAME = "FieldType";

        Node GetNode { get; }

        void FieldTypeSetupPorts<T>(Node.IPortDefinitionContext ctx);
    }

    internal static class InterfaceFieldTypeDefinitionExtension
    {
        public static void DefineFieldTypeOptions(this IFieldTypeDefinition obj, Node.IOptionDefinitionContext context)
        {
            context.AddOption<FieldType>(IFieldTypeDefinition.FIELD_TYPE_NAME)
                .Delayed()
                .Build();
        }

        public static void DefineFieldTypePorts(this IFieldTypeDefinition obj, Node.IPortDefinitionContext context)
        {
            FieldType fieldType = obj.GetNode.GetNodeOptionValue<FieldType>(IFieldTypeDefinition.FIELD_TYPE_NAME);

            switch (fieldType)
            {
                case FieldType.Int:
                    obj.FieldTypeSetupPorts<int>(context);
                    break;
                case FieldType.Bool:
                    obj.FieldTypeSetupPorts<bool>(context);
                    break;
                case FieldType.String:
                    obj.FieldTypeSetupPorts<string>(context);
                    break;
            }
        }
    }
}
using JetBrains.Annotations;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.GTKExtensions
{
    public static class InterfaceNodeExtension
    {
        public static T GetInputPortValue<T>(this INode node, string portName)
        {
            var port = node.GetInputPortByName(portName);
            if (port == null) 
                return default;

            return port.GetValue<T>();
        }

        public static T GetNodeOptionValue<T>(this INode node, string optionName)
        {
            if (node is Node nodeModel)
            {
                var option = nodeModel.GetNodeOptionByName(optionName);
                if (option == null)
                    return default;

                return option.GetValue<T>();
            }

            return default;
        }

        [CanBeNull]
        public static INode GetNextNodeWithOutPort(this INode node, string portName)
        {
            var port = node.GetOutputPortByName(portName);
            return port.isConnected ? port.GetConnectedNode() : null;
        }
    }
}
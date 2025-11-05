using JetBrains.Annotations;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.GTKExtensions
{
    public static class InterfacePortExtension
    {
        public static T GetValue<T>(this IPort port)
        {
            if (port == null)
            {
                return default;
            }

            T value = default;

            if (port.isConnected)
            {
                switch (port.firstConnectedPort.GetNode())
                {
                    case IVariableNode variableNode:
                        variableNode.variable.TryGetDefaultValue(out value);
                        return value;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue(out value);
                        return value;
                    default:
                        return default;
                }
            }

            port.TryGetValue(out value);
            return value;
        }

        [CanBeNull]
        public static INode GetConnectedNode(this IPort port)
        {
            return port.isConnected ? port.firstConnectedPort.GetNode() : null;
        }
    }
}
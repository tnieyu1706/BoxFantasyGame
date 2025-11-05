using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.GTKExtensions
{
    public static class InterfaceNodeOptionExtension
    {
        public static T GetValue<T>(this INodeOption option)
        {
            if (option.TryGetValue(out T value))
            {
                return value;
            }

            return default;
        }
    }
}
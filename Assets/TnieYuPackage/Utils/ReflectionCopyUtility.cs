using System;
using System.Reflection;

namespace TnieYuPackage.Utils
{
    public static class ReflectionCopyUtility
    {
        public static void ShallowCopy(object source, object target, bool checkedType = false)
        {
            if (source == null || target == null)
                throw new ArgumentNullException();

            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            if (checkedType && !targetType.IsAssignableFrom(sourceType) && !sourceType.IsAssignableFrom(targetType))
                throw new InvalidOperationException("Source and target types are not compatible.");

            // Lấy tất cả field (public + private, instance)
            var fields = sourceType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.IsStatic || field.IsInitOnly)
                    continue; // bỏ static và readonly

                var targetField = targetType.GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (targetField == null)
                    continue;

                var value = field.GetValue(source);
                targetField.SetValue(target, value);
            }
        }
    }
}
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Identify
{
    /// <summary>
    /// Flag provide GetIdentify method support.
    /// And hint required class must ve have [_IdentifyStorageFlag] to tracking.
    /// </summary>
    public interface IIdentifyFlagSupport
    {
    }

    public static class InterfaceIdentifyFlagSupportExtension
    {
        public static string GetIdentify(this IIdentifyFlagSupport iIdentifyFlagSupport)
        {
            var flag = iIdentifyFlagSupport.GetType().GetAttribute<TypeIdentifyStorageFlagAttribute>();
            if (flag == null)
            {
                Debug.LogWarningFormat("{0} dont have attribute a storage flag", iIdentifyFlagSupport.GetType());
                return string.Empty;
            }

            var storageIdField = flag.GetType()
                .GetField(
                    nameof(TypeIdentifyStorageFlagAttribute.StorageIdentify),
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic
                );

            if (storageIdField == null)
            {
                Debug.LogWarning(
                    $"{flag.GetType().FullName} don't found {nameof(TypeIdentifyStorageFlagAttribute.StorageIdentify)} field");
                return string.Empty;
            }

            var id = storageIdField.GetValue(flag) as string;

            return storageIdField + "/" + id;
        }
    }
}
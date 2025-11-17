using System.Reflection;
using UnityEngine;

namespace Systems.IdentifySystem.TypeIdentify
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
        public static bool TryToGetFlagAttribute(this IIdentifyFlagSupport iIdentifyFlagSupport,
            out TypeIdentifyStorageFlagAttribute flag)
        {
            flag = iIdentifyFlagSupport.GetType().GetCustomAttribute<TypeIdentifyStorageFlagAttribute>();
            
            if (flag == null)
            {
                Debug.LogWarningFormat("{0} dont have attribute a storage flag", iIdentifyFlagSupport.GetType());
                return false;
            }

            return true;
        }
        
        public static string GetIdentify(this IIdentifyFlagSupport iIdentifyFlagSupport)
        {
            if (TryToGetFlagAttribute(iIdentifyFlagSupport, out TypeIdentifyStorageFlagAttribute flag))
            {
                return flag.Identify;
            }
            
            Debug.LogWarningFormat("{0} dont have attribute a flag", iIdentifyFlagSupport.GetType());
            return string.Empty;
        }

        public static string GetStorageAndTypeIdentify(this IIdentifyFlagSupport iIdentifyFlagSupport)
        {
            TryToGetFlagAttribute(iIdentifyFlagSupport, out TypeIdentifyStorageFlagAttribute flag);

            if (flag == null)
            {
                return string.Empty;
            }

            var storageIdentify = flag.GetActualStorageIdentify();

            if (string.IsNullOrEmpty(storageIdentify))
            {
                return string.Empty;
            }

            var id = GetIdentify(iIdentifyFlagSupport);

            if (string.IsNullOrEmpty(id))
            {
                return storageIdentify;
            }

            return storageIdentify + "/" + id;
        }
    }
}
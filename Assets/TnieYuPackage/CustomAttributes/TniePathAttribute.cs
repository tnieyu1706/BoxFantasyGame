using UnityEngine;

namespace TnieYuPackage.CustomAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class TniePathAttribute : PropertyAttribute
    {
        public System.Type AssetType;
        public string[] Filters;

        public TniePathAttribute(System.Type assetType, params string[] filters)
        {
            AssetType = assetType;
            this.Filters = filters;
        }
    }
}
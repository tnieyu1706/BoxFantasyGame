using UnityEngine;

namespace TnieYuPackage.CustomAttributes
{
    public class TniePathAttribute : PropertyAttribute
    {
        public System.Type AssetType;

        public TniePathAttribute(System.Type assetType)
        {
            AssetType = assetType;
        }
    }
}
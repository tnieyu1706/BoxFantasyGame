using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TnieYuPackage.CustomAttributes.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TnieLayerMaskDropdownAttribute))]
    public class LayerMaskDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Lấy tất cả tên layer hiện có
            string[] layerNames = InternalEditorUtility.layers;
            int[] layerIndices = new int[layerNames.Length];

            for (int i = 0; i < layerNames.Length; i++)
                layerIndices[i] = LayerMask.NameToLayer(layerNames[i]);

            // Convert giá trị hiện tại sang dạng tạm để hiển thị MaskField
            int currentMask = property.intValue;
            int displayMask = 0;

            for (int i = 0; i < layerIndices.Length; i++)
            {
                if ((currentMask & (1 << layerIndices[i])) != 0)
                    displayMask |= 1 << i;
            }

            // Vẽ dropdown
            int newDisplayMask = EditorGUI.MaskField(position, label, displayMask, layerNames);

            // Chuyển kết quả người dùng chọn về layer index thật
            int newMask = 0;
            for (int i = 0; i < layerIndices.Length; i++)
            {
                if ((newDisplayMask & (1 << i)) != 0)
                    newMask |= 1 << layerIndices[i];
            }

            property.intValue = newMask;
        }
    }
}
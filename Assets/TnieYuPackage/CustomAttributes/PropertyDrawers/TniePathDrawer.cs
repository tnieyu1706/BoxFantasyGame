using UnityEditor;
using UnityEngine;

namespace TnieYuPackage.CustomAttributes.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TniePathAttribute))]
    public class TniePathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (TniePathAttribute)attribute;
            EditorGUI.BeginProperty(position, label, property);

            // Lấy object hiện tại từ path (nếu có)
            Object currentObj = null;
            if (!string.IsNullOrEmpty(property.stringValue))
            {
                currentObj = AssetDatabase.LoadAssetAtPath(property.stringValue, attr.AssetType);
            }

            // Label “type required”
            string typeName = attr.AssetType != null ? attr.AssetType.Name : "Object";
            label.text += $" ({typeName})";

            // Tính toán layout
            Rect fieldRect = new Rect(position.x, position.y, position.width - 90, position.height);
            Rect pingRect = new Rect(position.x + position.width - 85, position.y, 40, position.height);
            Rect clearRect = new Rect(position.x + position.width - 45, position.y, 45, position.height);

            // Drag field (hiển thị object hiện tại)
            EditorGUI.BeginChangeCheck();
            Object newObj = EditorGUI.ObjectField(fieldRect, label, currentObj, attr.AssetType, false);
            if (EditorGUI.EndChangeCheck())
            {
                if (newObj != null)
                {
                    string newPath = AssetDatabase.GetAssetPath(newObj);
                    property.stringValue = newPath;
                }
                else
                {
                    property.stringValue = string.Empty;
                }
            }

            // Ping button
            if (GUI.Button(pingRect, "Ping"))
            {
                if (currentObj != null)
                {
                    EditorGUIUtility.PingObject(currentObj);
                }
                else
                {
                    Debug.LogWarning($"No asset to ping for '{property.displayName}'.");
                }
            }

            // Clear button
            if (GUI.Button(clearRect, "X"))
            {
                property.stringValue = string.Empty;
            }

            EditorGUI.EndProperty();
        }
    }
}
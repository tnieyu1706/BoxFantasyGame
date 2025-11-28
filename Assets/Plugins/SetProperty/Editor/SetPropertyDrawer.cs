using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Amirebrahimi.SetProperty.Scripts;

namespace Amirebrahimi.SetProperty.Editor
{
    [CustomPropertyDrawer(typeof(SetPropertyAttribute))]
    public class SetPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SetPropertyAttribute setProp = (SetPropertyAttribute)attribute;
            object target = property.serializedObject.targetObject;

            // Backup value cũ (deep copy)
            object oldValue = GetValue(target, property);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, true);

            if (EditorGUI.EndChangeCheck())
            {
                // Áp lại serialized property vào object thật
                property.serializedObject.ApplyModifiedProperties();

                object newValue = GetValue(target, property);

                // Gọi setter của property thay vì field
                CallSetProperty(target, setProp.Name, newValue);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        // -------------------------------------------------------------
        // Lấy giá trị field thực từ object
        // -------------------------------------------------------------
        private object GetValue(object target, SerializedProperty prop)
        {
            string[] parts = prop.propertyPath.Split('.');
            object current = target;

            for (int i = 0; i < parts.Length; i++)
            {
                FieldInfo field = current.GetType().GetField(
                    parts[i],
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                );

                if (field == null) return null;
                current = field.GetValue(current);
            }

            return current;
        }

        // -------------------------------------------------------------
        // Trigger property setter
        // -------------------------------------------------------------
        private void CallSetProperty(object target, string propName, object newValue)
        {
            PropertyInfo pi = target.GetType().GetProperty(
                propName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (pi == null)
            {
                Debug.LogError($"[SetProperty] Cannot find property '{propName}'");
                return;
            }

            try
            {
                pi.SetValue(target, newValue, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SetProperty] Failed to set '{propName}': {ex.Message}");
            }
        }
    }
}

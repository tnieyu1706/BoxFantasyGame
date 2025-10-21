using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FilePathAttribute))]
    public class FilePathDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var filePathAttribute = attribute as FilePathAttribute;
            var root = new VisualElement();

            if (property.propertyType != SerializedPropertyType.String)
            {
                root.Add(new HelpBox("The FilePath Attribute can only be attached to a string", HelpBoxMessageType.Error));
                return root;
            }

            var filePath = property.stringValue;
            var propertyField = CreatePropertyField(property);
            var button = new Button();
            var buttonIcon = new Image { image = EditorGUIUtility.IconContent("d_Folder Icon").image };

            button.style.width = 40f;
            button.style.height = 20f;
            propertyField.style.flexGrow = 1f;
            root.style.flexDirection = FlexDirection.Row;

            button.Add(buttonIcon);
            root.Add(propertyField);
            root.Add(button);

            // Lấy text field bên trong propertyField (sau khi Unity build xong UI)
            TextField textField = null;
            ExecuteLater(propertyField, () => textField = propertyField.Q<TextField>());

            // Khi nhấn nút chọn file
            button.clicked += () =>
            {
                var selectedPath = EditorUtility.OpenFilePanel("Select file", "Assets", filePathAttribute.Filters);
                if (string.IsNullOrEmpty(selectedPath))
                    return;

                // Convert to relative path nếu có yêu cầu
                if (filePathAttribute.GetRelativePath && Path.IsPathFullyQualified(selectedPath))
                {
                    string projectRoot = Application.dataPath[..^"Assets".Length];
                    selectedPath = Path.GetRelativePath(projectRoot, selectedPath);
                }

                // Chỉ cập nhật nếu khác
                if (property.stringValue != selectedPath)
                {
                    property.stringValue = selectedPath;
                    property.serializedObject.ApplyModifiedProperties();

                    if (textField != null)
                        textField.SetValueWithoutNotify(selectedPath);
                }
            };

            // Đồng bộ khi load lại Inspector
            UpdateVisualElement(propertyField, () =>
            {
                if (property.hasMultipleDifferentValues)
                    return;

                if (textField != null && textField.value != property.stringValue)
                    textField.SetValueWithoutNotify(property.stringValue);
            });

            return root;
        }
    }
}

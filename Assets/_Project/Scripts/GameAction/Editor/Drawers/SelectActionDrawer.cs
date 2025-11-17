using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace Systems.GameAction.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SelectActionAttribute))]
    public class SelectActionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.alignItems = Align.Center;

            // Validate field type
            if (property.propertyType != SerializedPropertyType.String)
            {
                root.Add(new Label("SelectAction only works on string fields"));
                return root;
            }

            // Create dropdown
            var dropdown = new PopupField<string>();
            dropdown.label = property.displayName;
            dropdown.style.flexGrow = 1;

            // Fetch keys from storage
            var storage = ActionIdentifyStorage.Instance;
            if (storage == null)
            {
                root.Add(new Label("ActionIdentifyStorage.Instance = null"));
                return root;
            }

            var keys = new List<string>(storage.Datas.Dictionary.Keys);

            if (keys.Count == 0)
            {
                root.Add(new Label("No action keys found"));
                return root;
            }

            // Fill dropdown choices
            dropdown.choices = keys;

            // Set current value
            string currentValue = property.stringValue;
            if (!string.IsNullOrEmpty(currentValue) && keys.Contains(currentValue))
                dropdown.value = currentValue;
            else
                dropdown.value = keys[0];

            // When dropdown changes → assign back to property
            dropdown.RegisterValueChangedCallback(evt =>
            {
                property.serializedObject.Update();
                property.stringValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            root.Add(dropdown);

            return root;
        }
    }
}
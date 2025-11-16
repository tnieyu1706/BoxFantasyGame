using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.QuestSystem.QuestData.Editor
{
    [CustomPropertyDrawer(typeof(QuestStateDrawerAttribute))]
    public class QuestStateDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Root container
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.alignItems = Align.Center;

            // Left: the default Unity property field (foldout + SerializeReference UI)
            var field = new PropertyField(property, property.displayName);
            field.style.flexGrow = 1;
            root.Add(field);

            // Right: label for state name
            var stateLabel = new Label();
            stateLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            stateLabel.style.marginLeft = 8;
            stateLabel.style.width = 80;
            stateLabel.style.unityTextAlign = TextAnchor.MiddleRight;
            root.Add(stateLabel);

            // Update function
            void Refresh()
            {
                object obj = property.managedReferenceValue;
                stateLabel.text = obj != null ? obj.ToString() : "(null)";
            }

            // Refresh once
            Refresh();

            // Refresh when Unity updates the property
            field.RegisterValueChangeCallback(evt => Refresh());

            return root;
        }
    }
}
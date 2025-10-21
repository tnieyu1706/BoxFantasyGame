using UnityEditor;
using UnityEngine;

namespace Systems.FileData.JsonUtils.JsonDataStatic.Drawers
{
    [CustomPropertyDrawer(typeof(JsonDataStatic))]
    public class JsonDataStaticDrawer : PropertyDrawer
    {
        private const float BUTTON_WIDTH = 25f;
        private bool[] foldouts = new bool[100]; // simple foldout cache

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Padding & box
            GUI.Box(position, GUIContent.none);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            // Calculate rects
            Rect contentRect = new Rect(position.x + 6, position.y + 4, position.width - 12, EditorGUIUtility.singleLineHeight);

            // Draw Data Name
            var dataNameProp = property.FindPropertyRelative("dataName");
            EditorGUI.PropertyField(contentRect, dataNameProp, new GUIContent("Data Name"));
            contentRect.y += EditorGUIUtility.singleLineHeight + 4;

            // Draw Components
            var componentsProp = property.FindPropertyRelative("components");

            Rect foldoutRect = new Rect(contentRect.x, contentRect.y, contentRect.width, EditorGUIUtility.singleLineHeight);
            componentsProp.isExpanded = EditorGUI.Foldout(foldoutRect, componentsProp.isExpanded, "Components", true);
            contentRect.y += EditorGUIUtility.singleLineHeight + 2;

            if (componentsProp.isExpanded)
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < componentsProp.arraySize; i++)
                {
                    var elementProp = componentsProp.GetArrayElementAtIndex(i);
                    var elementRect = new Rect(contentRect.x, contentRect.y, contentRect.width, EditorGUIUtility.singleLineHeight);

                    // background box for element
                    var boxRect = new Rect(elementRect.x - 4, elementRect.y - 2, elementRect.width + 8, EditorGUIUtility.singleLineHeight + 4);
                    EditorGUI.HelpBox(boxRect, "", MessageType.None);

                    // Element foldout
                    foldouts[i] = EditorGUI.Foldout(new Rect(elementRect.x, elementRect.y, 100, elementRect.height), foldouts[i], $"Element {i}", true);

                    // Component field
                    var fieldRect = new Rect(elementRect.x + 100, elementRect.y, elementRect.width - 100, elementRect.height);
                    EditorGUI.PropertyField(fieldRect, elementProp, GUIContent.none);

                    contentRect.y += EditorGUIUtility.singleLineHeight + 2;

                    // nếu mở foldout thì vẽ property bên trong component
                    if (foldouts[i])
                    {
                        var component = elementProp.objectReferenceValue as Component;
                        if (component)
                        {
                            EditorGUI.indentLevel++;
                            var so = new SerializedObject(component);
                            var iterator = so.GetIterator();
                            iterator.NextVisible(true);
                            while (iterator.NextVisible(false))
                            {
                                if (iterator.name == "m_Script") continue;
                                var fieldHeight = EditorGUI.GetPropertyHeight(iterator, true);
                                var fieldRect2 = new Rect(contentRect.x, contentRect.y, contentRect.width, fieldHeight);
                                EditorGUI.PropertyField(fieldRect2, iterator, true);
                                contentRect.y += fieldHeight + 2;
                            }
                            so.ApplyModifiedProperties();
                            EditorGUI.indentLevel--;
                        }
                        else
                        {
                            EditorGUI.LabelField(new Rect(contentRect.x + 15, contentRect.y, contentRect.width, EditorGUIUtility.singleLineHeight),
                                "No Component assigned");
                            contentRect.y += EditorGUIUtility.singleLineHeight + 2;
                        }
                    }
                }

                // Add / Remove buttons
                Rect btnRect = new Rect(contentRect.x + contentRect.width - BUTTON_WIDTH * 2, contentRect.y + 2, BUTTON_WIDTH, EditorGUIUtility.singleLineHeight + 2);
                if (GUI.Button(btnRect, "+"))
                {
                    componentsProp.arraySize++;
                }

                btnRect.x += BUTTON_WIDTH + 4;
                if (GUI.Button(btnRect, "-"))
                {
                    if (componentsProp.arraySize > 0)
                        componentsProp.arraySize--;
                }

                contentRect.y += EditorGUIUtility.singleLineHeight + 8;
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 2; // base height
            var componentsProp = property.FindPropertyRelative("components");

            if (componentsProp.isExpanded)
            {
                for (int i = 0; i < componentsProp.arraySize; i++)
                {
                    height += EditorGUIUtility.singleLineHeight + 4; // header row
                    if (foldouts.Length > i && foldouts[i])
                    {
                        var elementProp = componentsProp.GetArrayElementAtIndex(i);
                        var comp = elementProp.objectReferenceValue as Component;
                        if (comp)
                        {
                            var so = new SerializedObject(comp);
                            var iterator = so.GetIterator();
                            iterator.NextVisible(true);
                            while (iterator.NextVisible(false))
                            {
                                if (iterator.name == "m_Script") continue;
                                height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
                            }
                        }
                        else height += EditorGUIUtility.singleLineHeight;
                    }
                }

                height += EditorGUIUtility.singleLineHeight + 8; // for Add/Remove
            }

            return height + 10;
        }
    }
}

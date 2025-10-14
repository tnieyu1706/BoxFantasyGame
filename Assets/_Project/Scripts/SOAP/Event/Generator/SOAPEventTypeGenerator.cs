#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace _Project.Scripts.SOAP.Event.Generator
{
    public class SOAPEventTypeGenerator : EditorWindow
    {
        private string folderPath = "Assets/_Project/Scripts/SOAP/Event/EventType";
        private string customTypeName = "";
        private string customType = "";
        private int selectedPresetIndex = 0;

        // Preset type list
        private readonly (string name, string type)[] presets =
        {
            ("Int", "int"),
            ("Float", "float"),
            ("Bool", "bool"),
            ("String", "string"),
            ("Vector2", "UnityEngine.Vector2"),
            ("Vector3", "UnityEngine.Vector3"),
            ("GameObject", "UnityEngine.GameObject"),
            ("Transform", "UnityEngine.Transform")
        };

        [MenuItem("Tools/SOAP/Event Type Generator")]
        private static void OpenWindow()
        {
            var window = GetWindow<SOAPEventTypeGenerator>("SOAP Event Type Generator");
            window.minSize = new Vector2(420, 250);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("SOAP Event Type Generator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Generate new SOAPEventGeneric<T> event types easily.", MessageType.Info);

            GUILayout.Space(10);

            // Output folder selection
            EditorGUILayout.LabelField("Output Folder:", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField(folderPath);
            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Output Folder", "Assets", "");
                if (!string.IsNullOrEmpty(selected))
                {
                    if (selected.StartsWith(Application.dataPath))
                        folderPath = "Assets" + selected.Substring(Application.dataPath.Length);
                    else
                        Debug.LogWarning("Selected folder must be inside Assets/");
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8);

            // Type presets
            EditorGUILayout.LabelField("Select Preset Type:", EditorStyles.boldLabel);
            selectedPresetIndex = EditorGUILayout.Popup(selectedPresetIndex, GetPresetNames());
            var (presetName, presetType) = presets[selectedPresetIndex];

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Or define custom type:", EditorStyles.boldLabel);
            customTypeName = EditorGUILayout.TextField("Type Name", customTypeName);
            customType = EditorGUILayout.TextField("C# Type", customType);

            GUILayout.Space(15);
            if (GUILayout.Button("Generate Event Type", GUILayout.Height(30)))
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    EditorUtility.DisplayDialog("Error", "Please select output folder.", "OK");
                    return;
                }

                string typeName = string.IsNullOrEmpty(customTypeName) ? presetName : customTypeName;
                string typeCs = string.IsNullOrEmpty(customType) ? presetType : customType;

                GenerateEventFile(typeName, typeCs);
            }
        }

        private void GenerateEventFile(string name, string type)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"Soap{name}Event.cs");
            if (File.Exists(filePath))
            {
                if (!EditorUtility.DisplayDialog("File Exists", $"File '{filePath}' already exists.\nOverwrite?", "Yes", "No"))
                    return;
            }

            string code = $@"
using UnityEngine;

namespace _Project.Scripts.SOAP.Event
{{
    [CreateAssetMenu(fileName = ""Soap{name}Event"", menuName = ""Scriptable Objects/SOAP/Event/{name}"")]
    public class Soap{name}Event : SoapEventGeneric<{type}> {{ }}
}}";

            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();

            Debug.Log($"✅ Generated SOAP Event Type: {filePath}");
        }

        private string[] GetPresetNames()
        {
            var names = new List<string>();
            foreach (var (name, _) in presets)
                names.Add(name);
            return names.ToArray();
        }
    }
}
#endif

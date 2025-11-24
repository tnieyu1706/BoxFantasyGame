#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace TnieYuPackage.SOAP.Event.Generator
{
    public class SoapEventTypeGenerator : EditorWindow
    {
        private string folderPath = "Assets/";
        private DefaultAsset folderAsset;

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

        [MenuItem("Tools/TnieYu/SOAP/Event - Generic Event Generator")]
        private static void OpenWindow()
        {
            var window = GetWindow<SoapEventTypeGenerator>("SOAP Event Type Generator");
            window.minSize = new Vector2(420, 250);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("SOAP Event Type Generator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Generate new SOAPEventGeneric<T> event types easily.", MessageType.Info);

            GUILayout.Space(10);

            // ===============================
            //     FOLDER SELECTION SECTION
            // ===============================
            EditorGUILayout.LabelField("Output Folder:", EditorStyles.boldLabel);

            // Drag & drop folder
            EditorGUI.BeginChangeCheck();
            folderAsset = (DefaultAsset)EditorGUILayout.ObjectField("Folder", folderAsset, typeof(DefaultAsset), false);
            if (EditorGUI.EndChangeCheck() && folderAsset != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(folderAsset);

                if (AssetDatabase.IsValidFolder(assetPath))
                {
                    folderPath = assetPath;
                }
                else
                {
                    Debug.LogWarning("Selected object is not a folder!");
                    folderAsset = null;
                }
            }

            // Manual path input + browse button
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField(folderPath);

            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Output Folder", "Assets", "");
                if (!string.IsNullOrEmpty(selected))
                {
                    if (selected.StartsWith(Application.dataPath))
                    {
                        folderPath = "Assets" + selected.Substring(Application.dataPath.Length);
                        folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
                    }
                    else
                        Debug.LogWarning("Selected folder must be inside Assets/");
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8);

            // ===============================
            //        TYPE PRESET UI
            // ===============================
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

        // FILE GENERATION
        private void GenerateEventFile(string name, string type)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"Soap{name}Event.cs");
            if (File.Exists(filePath))
            {
                if (!EditorUtility.DisplayDialog("File Exists", $"File '{filePath}' already exists.\nOverwrite?", "Yes",
                        "No"))
                    return;
            }

            string code = $@"
using UnityEngine;

namespace TnieYuPackage.SOAP.Event
{{
    [CreateAssetMenu(fileName = ""Soap{name}Event"", menuName = ""TnieYuPackage/SOAP/Event/{name}"")]
    public class Soap{name}Event : SoapEventGeneric<{type}> {{ }}
}}";

            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();

            Debug.Log($"✅ Generated SOAP Event Type: {filePath}");
        }

        private string[] GetPresetNames()
        {
            var list = new List<string>();
            foreach (var (name, _) in presets)
                list.Add(name);
            return list.ToArray();
        }
    }
}
#endif
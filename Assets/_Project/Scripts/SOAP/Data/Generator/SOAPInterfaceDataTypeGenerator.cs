#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Systems.SOAP.Data.Generator
{
    public class SOAPInterfaceDataTypeGenerator : EditorWindow
    {
        private string folderPath = "Assets/_Project/Scripts/SOAP/Data/InterfaceData";
        private string interfaceName = "";
        private string interfaceType = "";

        [MenuItem("Tools/SOAP/Interface Type Generator")]
        private static void OpenWindow()
        {
            var window = GetWindow<SOAPInterfaceDataTypeGenerator>("SOAP Interface Type Generator");
            window.minSize = new Vector2(420, 200);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("SOAP Interface Type Generator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Generate new SOAPInterfaceData<T> script files.", MessageType.Info);

            GUILayout.Space(10);

            // Folder selection
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

            GUILayout.Space(10);

            // Interface type inputs
            EditorGUILayout.LabelField("Interface Settings:", EditorStyles.boldLabel);
            interfaceName = EditorGUILayout.TextField("Interface Name", interfaceName);
            interfaceType = EditorGUILayout.TextField("C# Type", interfaceType);

            GUILayout.Space(15);
            GUI.enabled = !string.IsNullOrEmpty(interfaceName) && !string.IsNullOrEmpty(interfaceType);
            if (GUILayout.Button("Generate Interface Data", GUILayout.Height(30)))
                GenerateTypeFile(interfaceName, interfaceType);
            GUI.enabled = true;
        }

        private void GenerateTypeFile(string name, string type)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"SOAP{name}Data.cs";
            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                if (!EditorUtility.DisplayDialog("File Exists",
                    $"File '{fileName}' already exists.\nOverwrite?", "Yes", "No"))
                    return;
            }

            string code = $@"
using UnityEngine;

namespace Systems.SOAP.Data
{{
    [CreateAssetMenu(fileName = ""SOAP{name}Data"", menuName = ""Scriptable Objects/SOAP/InterfaceData/{name}"")]
    public class SOAP{name}Data : SOAPInterfaceData<{type}> {{ }}
}}";

            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();

            Debug.Log($"✅ Generated: {filePath}");
        }
    }
}
#endif

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TnieYuPackage.SOAP.Data.Generator
{
    public class SoapInterfaceDataTypeGenerator : EditorWindow
    {
        private string folderPath = "Assets/";
        private DefaultAsset folderAsset;

        private string interfaceName = "";
        private string interfaceType = "";

        [MenuItem("Tools/TnieYu/SOAP/Data - Interface Type Generator")]
        private static void OpenWindow()
        {
            var window = GetWindow<SoapInterfaceDataTypeGenerator>("SOAP Interface Type Generator");
            window.minSize = new Vector2(420, 200);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("SOAP Interface Type Generator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Generate new SOAPInterfaceData<T> script files.", MessageType.Info);

            GUILayout.Space(10);

            // ===========================
            //       FOLDER SECTION
            // ===========================
            EditorGUILayout.LabelField("Output Folder:", EditorStyles.boldLabel);

            // Drag & Drop folder
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

            // TextField + Browse button
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

            GUILayout.Space(10);

            // ===========================
            //      INTERFACE SETTINGS
            // ===========================
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

            string fileName = $"Soap{name}Data.cs";
            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                if (!EditorUtility.DisplayDialog(
                        "File Exists",
                        $"File '{fileName}' already exists.\nOverwrite?",
                        "Yes", "No"))
                    return;
            }

            string code = $@"
using UnityEngine;

namespace TnieYuPackage.SOAP.Data.Generator
{{
    [CreateAssetMenu(fileName = ""Soap{name}Data"", menuName = ""TnieYuPackage/SOAP/InterfaceData/{name}"")]
    public class Soap{name}Data : SoapInterfaceData<{type}> {{ }}
}}";

            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();

            Debug.Log($"✅ Generated: {filePath}");
        }
    }
}
#endif
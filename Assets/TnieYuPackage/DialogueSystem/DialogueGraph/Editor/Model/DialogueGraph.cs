using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model
{
    [Serializable]
    [Graph(AssetExtension)]
    internal class DialogueGraph : Graph
    {
        public const string AssetExtension = "dlgg";
        private const string default_graph_name = "DialogueGraph";

        [MenuItem("Assets/Create/Graphs/Dialogue Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>(default_graph_name);
        }
    }
}
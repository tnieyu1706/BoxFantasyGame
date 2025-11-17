using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Systems.QuestSystem.QuestFlow.QuestFlowGraph.Editor.Model
{
    [Serializable]
    [Graph(AssetExtension)]
    internal class QuestFlowGraph : Graph
    {
        public const string AssetExtension = "qfg";
        private const string default_graph_name = "QuestFlowGraph";

        [MenuItem("Assets/Create/QuestSystem/QuestFlowGraph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<QuestFlowGraph>(default_graph_name);
        }
    }
}
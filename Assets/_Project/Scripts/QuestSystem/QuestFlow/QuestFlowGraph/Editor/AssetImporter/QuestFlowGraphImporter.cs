using System.Collections.Generic;
using System.Linq;
using Systems.QuestSystem.QuestData;
using Systems.QuestSystem.QuestFlow.QuestFlowGraph.Editor.Model.Nodes;
using Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime;
using Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime.Nodes;
using TnieYuPackage.GTKExtensions;
using TnieYuPackage.Utils;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Systems.QuestSystem.QuestFlow.QuestFlowGraph.Editor.AssetImporter
{
    [ScriptedImporter(1, Model.QuestFlowGraph.AssetExtension)]
    public class QuestFlowGraphImporter : ScriptedImporter
    {
        private Dictionary<INode, QuestNodeRuntime> lookupTables;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            Model.QuestFlowGraph graph = GraphDatabase.LoadGraphForImporter<Model.QuestFlowGraph>(ctx.assetPath);

            if (graph == null)
            {
                Debug.LogError($"Could not load quest flow graph from {ctx.assetPath}");
                return;
            }

            QuestFlowGraphRuntime graphRuntime = ScriptableObject.CreateInstance<QuestFlowGraphRuntime>();
            //handle
            lookupTables ??= new();

            //first handle init nodeRuntime ensure own Quest.
            foreach (var node in graph.GetNodes())
            {
                lookupTables[node] = InitializeNodeModelToNodeRuntime(node);
            }

            //handle Pre/Next Quests
            foreach (var node in graph.GetNodes())
            {
                SetupDataForNode(node);
            }

            graphRuntime.nodes = lookupTables.Values.ToList();

            ctx.AddObjectToAsset("RuntimeAsset", graphRuntime);
            ctx.SetMainObject(graphRuntime);
        }

        private QuestNodeRuntime InitializeNodeModelToNodeRuntime(INode nodeModel)
        {
            var questData = nodeModel.GetNodeOptionValue<Quest>(QuestNode.QUEST_OPTION_NAME);
            if (questData == null)
            {
                Debug.LogError($"Could not find quest data for {nodeModel}");
                return null;
            }

            return new QuestNodeRuntime()
            {
                id = SerializableGuid.NewGuid(),
                quest = questData
            };
        }

        private void SetupDataForNode(INode nodeModel)
        {
            //get next Quests
            List<IPort> nextQuestPorts = new();
            var outPort = nodeModel.GetOutputPortByName(QuestNode.NEXTQUEST_NAME);
            if (outPort.isConnected)
            {
                outPort.GetConnectedPorts(nextQuestPorts);
            }

            List<Quest> nextQuests = new();
            foreach (var nextQuestPort in nextQuestPorts)
            {
                nextQuests.Add(lookupTables[nextQuestPort.GetNode()].quest);
            }

            //get pre Quests
            List<IPort> preQuestPorts = new();
            var preQuestNumber = nodeModel.GetNodeOptionValue<int>(QuestNode.NUMBER_PREQUEST_NAME);
            IPort preQuestPortRaw;
            if (preQuestNumber > 0)
            {
                for (int i = 0; i < preQuestNumber; i++)
                {
                    preQuestPortRaw = nodeModel.GetInputPortByName(QuestNode.PREQUEST_NAME + i);
                    if (preQuestPortRaw.isConnected)
                    {
                        preQuestPorts.Add(preQuestPortRaw.firstConnectedPort);
                    }
                }
            }

            List<Quest> preQuests = new();
            foreach (var preQuestPort in preQuestPorts)
            {
                if (preQuestPort.isConnected)
                {
                    preQuests.Add(lookupTables[preQuestPort.GetNode()].quest);
                }
            }
            
            //write data
            QuestNodeRuntime nodeRuntime = lookupTables[nodeModel];
            nodeRuntime.preQuests = preQuests;
            nodeRuntime.nextQuests = nextQuests;
        }
    }
}
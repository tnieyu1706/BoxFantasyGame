using System.Collections.Generic;
using System.Linq;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Generals;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes;
using TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes;
using TnieYuPackage.GTKExtensions;
using TnieYuPackage.Utils;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.AssetImporter
{
    [ScriptedImporter(1, Model.DialogueGraph.AssetExtension)]
    public class DialogueGraphImporter : ScriptedImporter
    {
        private Dictionary<INode, SerializableGuid> nodesLookup = new();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            Model.DialogueGraph graph = GraphDatabase.LoadGraphForImporter<Model.DialogueGraph>(ctx.assetPath);

            if (graph == null)
            {
                Debug.LogError($"Could not load graph from path: {ctx.assetPath}");
                return;
            }

            var startNode = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNode == null)
            {
                Debug.LogError($"Could not load start node from path: {ctx.assetPath}");
                return;
            }

            DialogueGraphRuntime graphRuntime = ScriptableObject.CreateInstance<DialogueGraphRuntime>();
            
            //configurations setup
            var isBackup = startNode.GetNodeOptionValue<bool>(StartNode.IS_BACKUP_NAME);
            graphRuntime.isBackup = isBackup;

            foreach (var node in graph.GetNodes().OfType<FieldNode>())
            {
                nodesLookup[node] = SerializableGuid.NewGuid();
                FieldNodeRuntime fieldNodeRuntime = TranslateFieldNodeToRuntime(node);
                
                graphRuntime.Fields.Add(fieldNodeRuntime);
                
                if (isBackup)
                {
                    graphRuntime.datasBackup.Add(fieldNodeRuntime.data);
                }
            }

            foreach (var node in graph.GetNodes().OfType<BaseNode>())
            {
                nodesLookup[node] = SerializableGuid.NewGuid();
            }
            
            // Debug.Log(nodesLookup.Count);

            NodeRuntime runtimeNode;
            
            INode currentNode;
            Queue<INode> queue = new();
            currentNode = startNode.GetNextNodeWithOutPort(BaseNode.EXECUTION_PORT_DEFAULT_NAME);
            if (currentNode != null)
                queue.Enqueue(currentNode);

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                runtimeNode = TranslateNodeModelToRuntimeNode(currentNode);
                graphRuntime.Nodes.Add(runtimeNode);
            
                foreach (var nextNode in GetNextNode(currentNode))
                {
                    if (!CheckNodeExist(graphRuntime.NodesDict.Values, nextNode))
                    {
                        queue.Enqueue(nextNode);
                    }
                }
            }

            ctx.AddObjectToAsset("RuntimeAsset", graphRuntime);
            ctx.SetMainObject(graphRuntime);
        }

        private bool CheckNodeExist(IEnumerable<NodeRuntime> nodes, INode node)
        {
            return nodes.Select(n => n.id).Contains(nodesLookup[node]);
            
        }

        private List<INode> GetNextNode(INode node)
        {
            var resultNodes = new List<INode>();

            if (node is ChoiceNode choiceNode)
            {
                int choiceNumber = choiceNode.GetNodeOptionValue<int>(ChoiceNode.CHOICE_NUMBER_NAME);

                INode nextNode;
                for (int i = 0; i < choiceNumber; i++)
                {
                    nextNode = choiceNode.GetNextNodeWithOutPort("Choice" + BaseNode.EXECUTION_PORT_DEFAULT_NAME + i);
                    if (nextNode != null)
                    {
                        resultNodes.Add(nextNode);
                    }
                }
            }
            else if (node is CompareNode compareNode)
            {
                INode trueNextNode = compareNode.GetNextNodeWithOutPort(CompareNode.TRUE_EXECUTION_PORT_NAME);
                if (trueNextNode != null)
                    resultNodes.Add(trueNextNode);
                
                INode falseNextNode = compareNode.GetNextNodeWithOutPort(CompareNode.FALSE_EXECUTION_PORT_NAME);
                if (falseNextNode != null)
                    resultNodes.Add(falseNextNode);
            }
            else
            {
                INode nextNode = node.GetNextNodeWithOutPort(BaseNode.EXECUTION_PORT_DEFAULT_NAME);
                
                if (nextNode != null)
                    resultNodes.Add(nextNode);
            }

            return resultNodes;
        }

        private FieldNodeRuntime TranslateFieldNodeToRuntime(FieldNode node)
        {
            SerializableGuid nodeId = nodesLookup[node];
            object defaultValue = node.GetInputPortValue<object>(FieldNode.VALUE_FIELD_INPUT_NAME);
            FieldType fieldType = node.GetNodeOptionValue<FieldType>(IFieldTypeDefinition.FIELD_TYPE_NAME);
            if (defaultValue != null)
            {
                node.fieldValue = defaultValue;
            }
            
            return new FieldNodeRuntime(nodeId, fieldType, node.fieldValue);
        }

        private NodeRuntime TranslateNodeModelToRuntimeNode(INode nodeModel)
        {
            //Setup value.
            NodeRuntime nodeRuntime = null;
            SerializableGuid nodeId = nodesLookup[nodeModel];
            SerializableGuid nextNodeId = SerializableGuid.Empty;

            //execute.

            INode nextNode;
            FieldType fieldType;
            
            switch (nodeModel)
            {
                case CompareNode compareNode:
                    SerializableGuid aFieldId = SerializableGuid.Empty;
                    SerializableGuid bFieldId = SerializableGuid.Empty;

                    SerializableGuid trueNextNodeId = SerializableGuid.Empty;
                    SerializableGuid falseNextNodeId = SerializableGuid.Empty;

                    CompareOperator compareOperator =
                        compareNode.GetNodeOptionValue<CompareOperator>(CompareNode.COMPARE_OPERATOR_NAME);
                    fieldType =
                        compareNode.GetNodeOptionValue<FieldType>(IFieldTypeDefinition.FIELD_TYPE_NAME);

                    var aFieldPort = compareNode.GetInputPortByName(CompareNode.A_FIELD_NAME);
                    var bFieldPort = compareNode.GetInputPortByName(CompareNode.B_FIELD_NAME);

                    if (aFieldPort.isConnected)
                    {
                        aFieldId = nodesLookup[aFieldPort.GetConnectedNode()!];
                    }

                    if (bFieldPort.isConnected)
                    {
                        bFieldId = nodesLookup[bFieldPort.GetConnectedNode()!];
                    }

                    INode trueNextNode = compareNode.GetNextNodeWithOutPort(CompareNode.TRUE_EXECUTION_PORT_NAME);
                    INode falseNextNode = compareNode.GetNextNodeWithOutPort(CompareNode.FALSE_EXECUTION_PORT_NAME);

                    if (trueNextNode != null)
                    {
                        trueNextNodeId = nodesLookup[trueNextNode];
                    }

                    if (falseNextNode != null)
                    {
                        falseNextNodeId = nodesLookup[falseNextNode];
                    }

                    nodeRuntime = new CompareNodeRuntime(nodeId, fieldType, compareOperator)
                    {
                        aFieldId = aFieldId,
                        bFieldId = bFieldId,
                        trueNextId = trueNextNodeId,
                        falseNextId = falseNextNodeId
                    };

                    break;
                case BaseDialogueNode baseDialogueNode:
                    var character = baseDialogueNode.GetInputPortValue<string>(BaseDialogueNode.CHARACTER_FIELD_NAME);
                    var content = baseDialogueNode.GetInputPortValue<string>(BaseDialogueNode.CONTENT_FIELD_NAME);

                    if (baseDialogueNode is DialogueNode dialogueNode)
                    {
                        nextNode = dialogueNode.GetNextNodeWithOutPort(BaseNode.EXECUTION_PORT_DEFAULT_NAME);
                        if (nextNode != null)
                        {
                            nextNodeId = nodesLookup[nextNode];
                        }

                        nodeRuntime = new DialogueNodeRuntime(nodeId, character, content)
                        {
                            nextId = nextNodeId
                        };

                        break;
                    }

                    if (baseDialogueNode is ChoiceNode choiceNode)
                    {
                        var choiceNumber = choiceNode.GetNodeOptionValue<int>(ChoiceNode.CHOICE_NUMBER_NAME);
                        List<ChoiceOption> choices = new();
                        
                        string choiceText;
                        SerializableGuid choiceNextId;
                        INode nextChoiceNode;
                        for (int i = 0; i < choiceNumber; i++)
                        {
                            choiceText = choiceNode.GetInputPortValue<string>(ChoiceNode.CHOICE_TEXT_DEFAULT_NAME + i);
                            nextChoiceNode =
                                choiceNode.GetNextNodeWithOutPort("Choice" + BaseNode.EXECUTION_PORT_DEFAULT_NAME + i);
                            if (nextChoiceNode != null)
                            {
                                choiceNextId = nodesLookup[nextChoiceNode];
                            }
                            else
                            {
                                choiceNextId = SerializableGuid.Empty;
                            }

                            choices.Add(new ChoiceOption
                                {
                                    text = choiceText,
                                    nextId = choiceNextId
                                }
                            );
                        }

                        nodeRuntime =
                            new ChoiceNodeRuntime(nodeId, character, content, choices);

                        break;
                    }

                    break;
                case FlowNode flowNode:
                    nextNode = flowNode.GetNextNodeWithOutPort(BaseNode.EXECUTION_PORT_DEFAULT_NAME);

                    if (nextNode != null)
                    {
                        nextNodeId = nodesLookup[nextNode];
                    }

                    if (flowNode is MethodNode methodNode)
                    {
                        var methodName = methodNode.GetInputPortValue<string>(MethodNode.METHOD_PORT_NAME);

                        nodeRuntime = new MethodNodeRuntime(nodeId, methodName)
                        {
                            NextId = nextNodeId
                        };

                        break;
                    }

                    if (flowNode is SetFieldNode setFieldNode)
                    {
                        object valueSet = setFieldNode.GetInputPortValue<object>(SetFieldNode.FIELD_VALUE_SET_NAME);
                        fieldType =
                            setFieldNode.GetNodeOptionValue<FieldType>(IFieldTypeDefinition.FIELD_TYPE_NAME);
                        
                        SerializableGuid fieldNodeId = SerializableGuid.Empty;
                        var fieldNodePort =
                            setFieldNode
                                .GetInputPortByName(SetFieldNode.FIELD_IDENTITY_NAME);

                        if (fieldNodePort.isConnected)
                        {
                            var fieldNode = fieldNodePort.firstConnectedPort.GetNode();

                            if (fieldNode is FieldNode)
                                fieldNodeId = nodesLookup[fieldNode];
                        }

                        nodeRuntime = new SetFieldNodeRuntime(nodeId, fieldType, valueSet)
                        {
                            NextId = nextNodeId,
                            fieldId = fieldNodeId
                        };

                        break;
                    }

                    break;
                default:
                    Debug.LogError($"Could not translate node to runtime {nodeId}");
                    nodeRuntime = null;
                    break;
            }

            return nodeRuntime;
        }
    }
}
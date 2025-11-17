using System.Collections.Generic;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts;
using EditorAttributes;
using JetBrains.Annotations;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using TnieYuPackage.Utils;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Void = EditorAttributes.Void;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime
{
    public class DialogueGraphDirector : SingletonBehavior<DialogueGraphDirector>
    {
        #region Properties

        [Required] public DialogueGraphRuntime graphRuntime;

        private Dictionary<SerializableGuid, NodeRuntime> nodesLookup;
        public NodeRuntime CurrentNode;

        [FormerlySerializedAs("dialoguePanelManager")] [Required]
        public DialogueGraphPanelManager dialogueGraphPanelManager;

        public SerializableDictionary<string, UnityEvent> methodsBinding = new();

        #endregion

        #region Buttons

        [ButtonField(nameof(StartDialogue))] public Void StartDialogueButton;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            
            if (dialogueGraphPanelManager == null)
            {
                Debug.LogError("Dialogue Graph Manager is null");
                return;
            }

            RefreshGraphRuntimeData();
        }

        private void StartDialogue()
        {
            dialogueGraphPanelManager.ShowDialogue();
            RefreshGraphRuntimeData();
            dialogueGraphPanelManager.UpdateUI(CurrentNode);
        }

        #region RegistryEvents

        private void OnEnable()
        {
            //Handle global
            DialogueNodeRuntime.Handle += HandleBaseDialogueNode;
            ChoiceNodeRuntime.Handle += HandleBaseDialogueNode;
            MethodNodeRuntime.Handle += HandleMethodNode;
            SetFieldNodeRuntime.Handle += HandleSetFieldNode;
            CompareNodeRuntime.Handle += HandleCompareNode;

            //DialoguePanelManager require event
            if (dialogueGraphPanelManager != null)
            {
                dialogueGraphPanelManager.MoveNext += OnMoveNext;
                dialogueGraphPanelManager.MoveNextChoice += OnMoveNextChoice;
            }
        }

        private void OnDisable()
        {
            //Handle global
            DialogueNodeRuntime.Handle -= HandleBaseDialogueNode;
            ChoiceNodeRuntime.Handle -= HandleBaseDialogueNode;
            MethodNodeRuntime.Handle -= HandleMethodNode;
            SetFieldNodeRuntime.Handle -= HandleSetFieldNode;
            CompareNodeRuntime.Handle -= HandleCompareNode;

            //DialoguePanelManager require event
            if (dialogueGraphPanelManager != null)
            {
                dialogueGraphPanelManager.MoveNext -= OnMoveNext;
                dialogueGraphPanelManager.MoveNextChoice -= OnMoveNextChoice;
            }
        }

        #endregion

        #region DialoguePanel requirements

        private void OnMoveNext()
        {
            if (CurrentNode is DialogueNodeRuntime dialogueNodeRuntime)
            {
                SetNextNode(dialogueNodeRuntime.nextId);

                if (CurrentNode != null)
                    NodeRuntime.GetActualHandleContext(CurrentNode)?.Invoke(CurrentNode);
            }
            else
            {
                dialogueGraphPanelManager.HideDialogue();
            }
        }


        private void OnMoveNextChoice(int choiceIndex)
        {
            if (CurrentNode is ChoiceNodeRuntime choiceNodeRuntime)
            {
                SetNextNode(choiceNodeRuntime.choices[choiceIndex].nextId);

                if (CurrentNode != null)
                    NodeRuntime.GetActualHandleContext(CurrentNode)?.Invoke(CurrentNode);
            }
            else
            {
                dialogueGraphPanelManager.HideDialogue();
            }
        }

        #endregion

        #region NodeRuntimeHandles

        private void HandleBaseDialogueNode(NodeRuntime node)
        {
            dialogueGraphPanelManager.UpdateUI(node);
        }

        private void HandleMethodNode(NodeRuntime node)
        {
            if (node is MethodNodeRuntime methodNode)
                HandleFlowNode(methodNode);
        }

        private void HandleSetFieldNode(NodeRuntime node)
        {
            if (node is SetFieldNodeRuntime setFieldNode)
                HandleFlowNode(setFieldNode);
        }

        private void HandleCompareNode(NodeRuntime node)
        {
            if (node is CompareNodeRuntime compareNode)
                HandleFlowNode(compareNode);
        }

        private void HandleFlowNode(IFlowNodeRuntime node)
        {
            node.Execute();

            SetNextNode(node.NextId);
            if (CurrentNode != null)
                NodeRuntime.GetActualHandleContext(CurrentNode)?.Invoke(CurrentNode);
            else
            {
                dialogueGraphPanelManager.HideDialogue();
            }
        }

        #endregion

        #region Script

        void RefreshGraphRuntimeData()
        {
            if (graphRuntime == null)
            {
                Debug.LogError("Graph Runtime is null");
                return;
            }

            DialogueGraphRuntime.CurrentGraphRuntime = graphRuntime;

            graphRuntime.BackupFieldData();

            nodesLookup = new();
            foreach (var node in graphRuntime.Nodes)
            {
                nodesLookup[node.id] = node;
            }

            foreach (var method in methodsBinding.Dictionary)
            {
                graphRuntime.MethodBindings.AddOrUpdate(method.Key, method.Value);
            }

            CurrentNode = graphRuntime.Nodes[0];
        }

        public void SetNextNode(SerializableGuid nodeId)
        {
            var node = LookupNode(nodeId);

            CurrentNode = node;
        }

        public void SetNextNode(NodeRuntime nodeRuntime)
        {
            CurrentNode = nodeRuntime;
        }

        [CanBeNull]
        public NodeRuntime LookupNode(SerializableGuid id)
        {
            nodesLookup.TryGetValue(id, out var node);
            return node;
        }

        public void SetDialogueGraph(DialogueGraphRuntime graph)
        {
            graphRuntime = graph;
            RefreshGraphRuntimeData();
        }

        #endregion
    }
}
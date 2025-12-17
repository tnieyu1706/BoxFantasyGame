using TnieYuPackage.DialogueSystem.DialogueSheet.Models;
using EditorAttributes;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using TnieYuPackage.FileData;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TnieYuPackage.DialogueSystem.DialogueSheet.Scripts
{
    public class DialogueSheetDirector : SingletonBehavior<DialogueSheetDirector>
    {
        #region Properties
        public TnieYuPackage.DialogueSystem.DialogueSheet.Models.DialogueSheet dialogueSheet;
        
        [FormerlySerializedAs("fileData")] [SerializeReference, AbstractSupport(typeof(IFileService))]
        public IFileService fileService;

        public SerializableDictionary<string, UnityEvent> actionMethods = new();
        
        [Required]
        public DialogueSheetPanelManager dialogueSheetPanelManager;

        public DialogueNode currentNode;
        
        #endregion

        [Button]
        public void StartDialogue()
        {
            dialogueSheetPanelManager.ShowDialogue();
            RefreshDialogueSheet();
            dialogueSheetPanelManager.UpdateUI(currentNode);
        }

        private void OnEnable()
        {
            if (dialogueSheetPanelManager != null)
            {
                dialogueSheetPanelManager.MoveNext += OnMoveNext;
                dialogueSheetPanelManager.MoveNextChoice += OnMoveNextChoice;
            }
        }

        private void OnDisable()
        {
            if (dialogueSheetPanelManager != null)
            {
                dialogueSheetPanelManager.MoveNext -= OnMoveNext;
                dialogueSheetPanelManager.MoveNextChoice -= OnMoveNextChoice;
            }
        }
        
        #region DialogueSheetPanel Requirements

        private void OnMoveNext()
        {
            string nodeSetId = currentNode.next;
            DialogueNode nodeSet = dialogueSheet.nodes.Dictionary.ContainsKey(nodeSetId) ? dialogueSheet.nodes[nodeSetId] : null;
            currentNode = nodeSet;
            
            dialogueSheetPanelManager.UpdateUI(currentNode);
        }

        private void OnMoveNextChoice(int choiceIndex)
        {
            string nodeSetId = currentNode.choices[choiceIndex].next;
            DialogueNode nodeSet = dialogueSheet.nodes.Dictionary.ContainsKey(nodeSetId) ? dialogueSheet.nodes[nodeSetId] : null;
            currentNode = nodeSet;
            
            dialogueSheetPanelManager.UpdateUI(currentNode);
        }
        
        #endregion

        [Button("RefreshDialogueSheetManual")]
        public void RefreshDialogueSheet()
        {
            var sheet = new TnieYuPackage.DialogueSystem.DialogueSheet.Models.DialogueSheet.Builder()
                .BuildDialogueNodes(fileService)
                .BuildActions(actionMethods.Dictionary)
                .Build();

            if (sheet == null)
            {
                Debug.LogWarning("No data loaded!");
                return;
            }

            dialogueSheet = sheet;
            
            //configurations base
            currentNode = dialogueSheet.nodes.data[0].value;
            
            TnieYuPackage.DialogueSystem.DialogueSheet.Models.DialogueSheet.Instance = this.dialogueSheet;
        }

        public void SetDialogueSheet(TnieYuPackage.DialogueSystem.DialogueSheet.Models.DialogueSheet dialogueSheetData)
        {
            this.dialogueSheet = dialogueSheetData;
            RefreshDialogueSheet();
        }
    }
}
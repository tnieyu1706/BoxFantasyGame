using System.Collections.Generic;
using System.Linq;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts
{
    public class DialogueGraphPanelManager : BaseDialoguePanelManager<NodeRuntime>
    {
        private bool isMoveNextRegistry = false;
        
        public override void UpdateUI(NodeRuntime node)
        {
            if (node == null || node is not IBaseDialogueNodeRuntime)
            {
                Debug.Log("Current node is null!");
                HideDialogue();

                return;
            }
            
            string characterName = string.Empty;
            string dialogueContent = string.Empty;
            if (node is DialogueNodeRuntime dialogueNode)
            {
                characterName = dialogueNode.Character;
                dialogueContent = dialogueNode.Content;

                if (!isMoveNextRegistry)
                {
                    contentManager.onPointerClick.AddListener(MoveNext);
                    isMoveNextRegistry = true;
                }
                choicePanelManager.DisableChoicePanel();
            }
            else if (node is ChoiceNodeRuntime choiceNode)
            {
                characterName = choiceNode.Character;
                dialogueContent = choiceNode.Content;
                List<string> choicesText = choiceNode.choices.Select(choice => choice.text).ToList();

                choicePanelManager.EnableChoicePanel(choicesText);

                if (isMoveNextRegistry)
                {
                    contentManager.onPointerClick.RemoveListener(MoveNext);
                    isMoveNextRegistry = false;
                }
            }

            characterManager.characterText.SetText(characterName);
            contentManager.contentText.SetText(dialogueContent);
        }
    }
}
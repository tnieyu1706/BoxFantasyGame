using System.Linq;
using TnieYuPackage.DialogueSystem.DialogueSheet.Models;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts;

namespace TnieYuPackage.DialogueSystem.DialogueSheet.Scripts
{
    public class DialogueSheetPanelManager : BaseDialoguePanelManager<DialogueNode>
    {
        private bool isMoveNextRegistry = false;
        public override void UpdateUI(DialogueNode node)
        {
            if (node != null)
            {
                //base field
                characterManager.characterText.SetText(node.characterName);
                contentManager.contentText.SetText(node.content);

                //action
                if (!string.IsNullOrEmpty(node.actionName))
                {
                    if (TnieYuPackage.DialogueSystem.DialogueSheet.Models.DialogueSheet.Instance.actions.Dictionary.ContainsKey(node.actionName))
                    {
                        TnieYuPackage.DialogueSystem.DialogueSheet.Models.DialogueSheet.Instance.actions[node.actionName]?.Invoke();
                    }
                }
                
                //next move.
                if (node.IsChoiceNode)
                {
                    choicePanelManager.EnableChoicePanel(node.choices.Select(c => c.text).ToList());
                    
                    //deactivate moveNext
                    if (isMoveNextRegistry)
                    {
                        contentManager.onPointerClick.RemoveListener(MoveNext);
                        isMoveNextRegistry = false;
                    }
                }
                else
                {
                    choicePanelManager.DisableChoicePanel();

                    if (!isMoveNextRegistry)
                    {
                        contentManager.onPointerClick.AddListener(MoveNext);
                        isMoveNextRegistry = true;
                    }
                }
            }
            else
            {
                HideDialogue();
            }
        }
    }
}
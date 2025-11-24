using UnityEngine;
using UnityEngine.Events;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts
{
    public abstract class BaseDialoguePanelManager<TNode> : MonoBehaviour
    {
        #region Properties

        public CharacterPanelManager characterManager;
        public ContentPanelManager contentManager;
        public ChoicePanelManager choicePanelManager;

        #endregion

        #region Fields

        public UnityAction MoveNext;
        public UnityAction<int> MoveNextChoice;

        #endregion

        protected virtual void Awake()
        {
            if (characterManager == null || contentManager == null || choicePanelManager == null)
            {
                Debug.LogWarning("Dialogue Panel Manager is null!");
                return;
            }
        }

        protected virtual void Start()
        {
            HideDialogue();
        }

        protected virtual void OnEnable()
        {
            choicePanelManager.OnChoiceSelect.AddListener(SelectChoice);
        }

        protected virtual void OnDisable()
        {
            choicePanelManager.OnChoiceSelect.RemoveListener(SelectChoice);
        }

        protected virtual void SelectChoice(int choiceIndex)
        {
            MoveNextChoice?.Invoke(choiceIndex);
        }

        public abstract void UpdateUI(TNode node);

        public virtual void ShowDialogue()
        {
            characterManager.gameObject.SetActive(true);
            contentManager.gameObject.SetActive(true);
            choicePanelManager.gameObject.SetActive(true);
        }

        public virtual void HideDialogue()
        {
            characterManager.gameObject.SetActive(false);
            contentManager.gameObject.SetActive(false);
            choicePanelManager.gameObject.SetActive(false);
        }
    }
}
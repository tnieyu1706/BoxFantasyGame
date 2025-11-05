using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts
{
    public class ChoiceElement : MonoBehaviour
    {
        public Button choiceButton;
        public TextMeshProUGUI choiceText;

        public ChoicePanelManager choiceManager;

        public void Awake()
        {
            if (choiceButton == null)
                choiceButton = GetComponent<Button>();
            if (choiceText == null)
                choiceText = GetComponentInChildren<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            choiceButton.onClick.AddListener(OnChoiceSelected);
        }

        private void OnDisable()
        {
            choiceButton.onClick.RemoveListener(OnChoiceSelected);
        }

        private void OnChoiceSelected()
        {
            if (choiceManager == null)
            {
                Debug.LogWarning("No choice manager attached to choice panel");
                return;
            }
            
            choiceManager.SelectChoice(gameObject);
        }
    }
}
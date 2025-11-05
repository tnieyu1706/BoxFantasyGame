using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts
{
    public class ChoicePanelManager : MonoBehaviour
    {
        public Transform ChoicePanel;
        public GameObject ChoiceElementPrefab;
        /// <summary>
        /// Registry event choice select with DialogueManager. to move next Node.
        /// </summary>
        public UnityEvent<int> OnChoiceSelect;

        #region ChoiceManager

        private List<GameObject> choiceElements;

        #endregion
        
        private int selectedIndex;

        void Awake()
        {
            InitializeChoicePanel();
        }

        private void InitializeChoicePanel()
        {
            //delete available.
            int childCount = ChoicePanel.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(ChoicePanel.GetChild(i).gameObject);
            }

            //init pooling
            choiceElements = new(GlobalConfigs.MAX_CHOICE);
            for (int i = 0; i < GlobalConfigs.MAX_CHOICE; i++)
            {
                GameObject choiceElement = Instantiate(ChoiceElementPrefab, ChoicePanel);
                choiceElement.GetComponent<ChoiceElement>().choiceManager = this;
                choiceElement.SetActive(false);
                choiceElements.Add(choiceElement);
            }
        }

        public void EnableChoicePanel(List<string> textChoices)
        {
            GameObject choiceElement;
            for (int i = 0; i < textChoices.Count; i++)
            {
                choiceElement = choiceElements[i];
                choiceElement.gameObject.SetActive(true);
                choiceElement.GetComponent<ChoiceElement>().choiceText.SetText(textChoices[i]);
            }
        }

        public void DisableChoicePanel()
        {
            foreach (var choice in choiceElements)
            {
                if (choice.gameObject.activeSelf)
                    choice.gameObject.SetActive(false);
            }
        }

        public void SelectChoice(GameObject choice)
        {
            if (choiceElements.Contains(choice))
            {
                selectedIndex = choiceElements.IndexOf(choice);
                OnChoiceSelect?.Invoke(selectedIndex);
            }
        }
    }
}
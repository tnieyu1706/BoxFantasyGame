using System.Collections.Generic;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;
using Logger = Backbone.Logger;

namespace Systems.Input
{
    public class InputUIManager : SceneSingletonBehaviour<InputUIManager>
    {
        #region Properties
        
        [SerializeField, Required] GameObject uiElementPrefab;
        [SerializeField, Required] Transform uiElementParent;
        
        #endregion

        private Dictionary<string, InputUIElement> uiElements;
        private InputUIElement uiElementTemp;

        protected override void Awake()
        {
            base.Awake();
            
            InitializeUIElements();
        }

        private void OnEnable()
        {
            PlayerInputReader.Instance.OnInputSubscribe += ShowInput;
            PlayerInputReader.Instance.OnInputUnsubscribe += HideInput;
        }

        private void OnDisable()
        {
            PlayerInputReader.Instance.OnInputSubscribe -= ShowInput;
            PlayerInputReader.Instance.OnInputUnsubscribe -= HideInput;
        }

        #region Initialize UI
        
        private void InitializeUIElements()
        {
            var inputInfos = PlayerInputReader.Instance.GetInputInfos();

            uiElements = new();
            foreach (var inputInfo in inputInfos)
            {
                uiElements[inputInfo.InputName] = CreateUIElement(inputInfo);
            }
        }

        private InputUIElement CreateUIElement(IInputInfo inputInfo)
        {
            GameObject uiObject = Instantiate(uiElementPrefab, uiElementParent);
            InputUIElement inputUIElement = uiObject.GetComponent<InputUIElement>();
            inputUIElement.inputText.SetText(inputInfo.InputName);
            inputUIElement.inputImage.sprite = inputInfo.InputIcon;

            uiObject.SetActive(false);
            
            return inputUIElement;
        }
        
        #endregion
        
        #region Event Handlers

        public void ShowInput(string inputName)
        {
            if (!uiElements.TryGetValue(inputName, out uiElementTemp))
            {
                Logger.Log($"Don't found UI element: {inputName}", category: "UI");
                return;
            }

            uiElementTemp.Show();
        }

        public void HideInput(string inputName)
        {
            if (!uiElements.TryGetValue(inputName, out uiElementTemp))
            {
                Logger.Log($"Don't found UI element: {inputName}", category: "UI");
                return;
            }
            
            uiElementTemp.Hide();
        }
        
        #endregion
        
    }
}
using System;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Input
{
    [CreateAssetMenu(fileName = "InterfaceInputReader", menuName = "Scriptable Objects/Input/InterfaceInputReader")]
    public class InterfaceInputReader : SingletonScriptable<InterfaceInputReader>, InputSystem.IInterfaceActions,
        IInputReader
    {
        public Action Tab;
        public Action Map;
        public Action<int> SelectItem;
        
        public void EnableActions()
        {
            if (IInputReader.InputSystem == null)
            {
                IInputReader.InputSystem = new InputSystem();
            }
            
            IInputReader.InputSystem.Interface.SetCallbacks(this);
            IInputReader.InputSystem.Interface.Enable();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InterfaceInputReaderInitialize()
        {
            PlayerInputReader.Instance.EnableActions();
        }
        
        public void OnTab(InputAction.CallbackContext context)
        {
            Tab?.Invoke();
        }

        public void OnMap(InputAction.CallbackContext context)
        {
            Map?.Invoke();
        }

        public void OnSelectItem(InputAction.CallbackContext context)
        {
            int index = context.control.name switch
            {
                "1" => 0,
                "2" => 1,
                "3" => 2,
                "4" => 3,
                "5" => 4,
                _ => -1
            };

            if (index == -1)
            {
                Debug.LogWarning("SelectItem button is not valid.");
                return;
            }
            
            SelectItem?.Invoke(index);
        }

        
    }
}
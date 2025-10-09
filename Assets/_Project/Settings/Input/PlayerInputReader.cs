using System;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Input
{
    public interface IInputReader
    {
        void EnableActions();

        public static InputSystem InputSystem;
    }

    [CreateAssetMenu(fileName = "PlayerInputReader", menuName = "Scriptable Objects/Input/PlayerInputReader")]
    public class PlayerInputReader : SingletonScriptable<PlayerInputReader>, InputSystem.IPlayerActions, IInputReader
    {
        public Action<Vector2> Move;
        public Action Interact;
        public Action Jump;
        public Action<Vector2> Look;
        public Action Drop;
        public Action Pickup;
        public Action PrimaryAction;
        public Action SecondaryAction;

        public void EnableActions()
        {
            if (IInputReader.InputSystem == null)
            {
                IInputReader.InputSystem = new InputSystem();
            }

            IInputReader.InputSystem.Player.SetCallbacks(this);
            IInputReader.InputSystem.Player.Enable();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void PlayerInputReaderInitialize()
        {
            PlayerInputReader.Instance.EnableActions();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Interact?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Jump?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnProp(InputAction.CallbackContext context)
        {
            Drop?.Invoke();
        }

        public void OnPickup(InputAction.CallbackContext context)
        {
            Pickup?.Invoke();
        }

        public void OnPrimaryAction(InputAction.CallbackContext context)
        {
            PrimaryAction?.Invoke();
        }

        public void OnSecondaryAction(InputAction.CallbackContext context)
        {
            SecondaryAction?.Invoke();
        }
    }
}
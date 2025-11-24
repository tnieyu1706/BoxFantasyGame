using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Input
{
    [CreateAssetMenu(fileName = "PlayerInputReader", menuName = "Scriptable Objects/Input/PlayerInputReader")]
    public class PlayerInputReader : BaseInputReader<PlayerInputReader>, InputSystem.IPlayerActions
    {
        public Action<Vector2> Move;
        public Action Jump;
        public Action<Vector2> Look;
        
        public InputInfo<Action> interact;
        public InputInfo<Action> drop;
        public InputInfo<Action> pickUp;
        public InputInfo<Action> primaryAction;
        public InputInfo<Action> secondaryAction;

        public override void EnableActions()
        {
            IInputReader.InputSystem ??= new InputSystem();

            IInputReader.InputSystem.Player.SetCallbacks(this);
            IInputReader.InputSystem.Player.Enable();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void PlayerInputReaderInitialize()
        {
            Instance.EnableActions();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                interact.Event?.Invoke();
            }
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
            drop.Event?.Invoke();
        }

        public void OnPickup(InputAction.CallbackContext context)
        {
            pickUp.Event?.Invoke();
        }

        public void OnPrimaryAction(InputAction.CallbackContext context)
        {
            primaryAction.Event?.Invoke();
        }

        public void OnSecondaryAction(InputAction.CallbackContext context)
        {
            secondaryAction.Event?.Invoke();
        }
    }
}
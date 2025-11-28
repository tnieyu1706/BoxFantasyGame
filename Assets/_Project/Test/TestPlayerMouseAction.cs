using Systems.GameAction;
using Systems.GameAction.GameActions;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using Systems.Input;
using UnityEngine;

namespace _Project.Test
{
    public class TestPlayerMouseAction : MonoBehaviour
    {
        public StaticObjectIdentify testSender;
        public StaticObjectIdentify testTarget;
        
        private void OnEnable()
        {
            PlayerInputReader.Instance.primaryAction.Event += HandleLeftClick;
            PlayerInputReader.Instance.secondaryAction.Event += HandleRightClick;
        }

        private void OnDisable()
        {
            PlayerInputReader.Instance.primaryAction.Event -= HandleLeftClick;
            PlayerInputReader.Instance.secondaryAction.Event -= HandleRightClick;
        }

        private void HandleLeftClick()
        {
            IGameAction gameAction = new DoSomethingAction();

            GameActionCommand command = new GameActionCommand(testSender, testTarget, gameAction);
            GameActionSystem.Instance.PushCommand(command);
        }

        private void HandleRightClick()
        {
            IGameAction gameAction = new DoSomething2Action();

            GameActionCommand command = new GameActionCommand(testSender, testTarget, gameAction);
            GameActionSystem.Instance.PushCommand(command);
        }
    }
}
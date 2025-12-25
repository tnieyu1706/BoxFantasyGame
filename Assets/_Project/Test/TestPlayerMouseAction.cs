using Systems.GameAction;
using Systems.GameAction.ActionStrategies;
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
            IGameActionStrategy gameActionStrategy = new DoSomethingGameActionStrategy();

            GameActionCommand command = new GameActionCommand(testSender, testTarget, gameActionStrategy, null);
            GameActionSystem.Instance.PushCommand(command);
        }

        private void HandleRightClick()
        {
            IGameActionStrategy gameActionStrategy = new DoSomethingGameActionStrategy();

            GameActionCommand command = new GameActionCommand(testSender, testTarget, gameActionStrategy, null);
            GameActionSystem.Instance.PushCommand(command);
        }
    }
}
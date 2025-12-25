using EditorAttributes;
using Systems.GameAction;
using Systems.GameAction.ActionStrategies;
using UnityEngine;

namespace _Project.Test
{
    public class TestGameActionRuntimeComponent : MonoBehaviour
    {
        public GameObject sender;
        public GameObject target;
        
        private DoSomethingGameActionStrategy doSomethingGameActionStrategy;

        private DoSomethingGameActionStrategy DoSomethingGameActionStrategy
        {
            get
            {
                doSomethingGameActionStrategy ??= new DoSomethingGameActionStrategy();
                
                return doSomethingGameActionStrategy;
            }
        }

        [Button]
        private void TestAction()
        {
            GameActionCommand command = new GameActionCommand(sender, target, DoSomethingGameActionStrategy, null);
            
            GameActionSystem.Instance.PushCommand(command);
        }
    }
}
using Systems.GameAction;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using TnieYuPackage.CustomAttributes;
using UnityEngine;

namespace _Project.Test
{
    public class DoSomethingComponent : MonoBehaviour
    {
        [SerializeReference, AbstractSupport(typeof(IGameAction))]
        public IGameAction action;

        public StaticObjectIdentify sender;
        public StaticObjectIdentify target;
        
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                ActivateAction();
            }
        }

        private void ActivateAction()
        {
            if (action == null) return;
            
            GameActionCommand actionCommand = new GameActionCommand(sender, target, action);
            GameActionSystem.Instance.PushCommand(actionCommand);
        }
    }
}
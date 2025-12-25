using UnityEngine;

namespace Systems.GameAction
{
    public class GameActionCommand
    {
        public object Sender;
        public object Target;
        public IGameActionStrategy ActionStrategy;
        public object ActionPayload;

        public GameActionCommand(object sender, object target, IGameActionStrategy actionStrategy, object actionPayload)
        {
            Sender = sender;
            Target = target;
            ActionStrategy = actionStrategy;
            ActionPayload = actionPayload;
        }

        public void Call()
        {
            if (ActionStrategy == null)
            {
                Debug.LogWarning("No action strategy set!");
                return;
            }
            
            ActionStrategy.Execute(ActionPayload, Sender, Target);
        }
    }
}
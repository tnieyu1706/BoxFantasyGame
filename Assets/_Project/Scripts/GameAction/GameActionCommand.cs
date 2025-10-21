using System;

namespace Systems.GameAction
{
    [Serializable]
    public class GameActionCommand
    {
        private IGameActionSubject sender;
        private IGameActionSubject target;
        private IGameAction action;

        public GameActionCommand(IGameActionSubject sender, IGameActionSubject target, IGameAction action)
        {
            this.sender = sender;
            this.target = target;
            this.action = action;
        }

        public void Trigger()
        {
            action.Execute(sender, target);
        }
    }
}
using System;
using Systems.GameAction.ActionStrategies;
using UnityEngine;

namespace Systems.GameAction.ActionCatchers
{
    [Serializable]
    public class DoSomethingGameActionCatcher : IGameActionCatcher
    {
        public string senderGameObjectNameCatching;
        public string targetGameObjectNameCatching;
        public const string ActionNameCatching = DoSomethingGameActionStrategy.DO_SOMETHING_ACTION_IDENTIFY_NAME;
        
        public Action OnCompleted { get; set; }

        public bool Catch(GameActionCommand actionCommand)
        {
            if (!((GameObject)actionCommand.Sender).name.Equals(senderGameObjectNameCatching))
                return false;
            
            if (!((GameObject)actionCommand.Target).name.Equals(targetGameObjectNameCatching))
                return false;

            return true;
        }
        
        public bool Format(GameActionCommand actionCommand)
        {
            if (actionCommand.Sender is not GameObject)
                return false;

            if (actionCommand.Target is not GameObject)
                return false;

            return actionCommand.ActionStrategy.ActionIdentifyName.Equals(ActionNameCatching);
        }
    }
}
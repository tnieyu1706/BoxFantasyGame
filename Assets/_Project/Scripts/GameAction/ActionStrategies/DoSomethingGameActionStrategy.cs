using System;
using UnityEngine;

namespace Systems.GameAction.ActionStrategies
{
    [Serializable]
    public class DoSomethingGameActionStrategy : IGameActionStrategy
    {
        public const string DO_SOMETHING_ACTION_IDENTIFY_NAME = "DoSomething";
        
        public string ActionIdentifyName => DO_SOMETHING_ACTION_IDENTIFY_NAME;
        public void Execute(object payload, object sender, object target)
        {
            if (sender is GameObject senderGObj)
            {
                Debug.Log(senderGObj.name);
            }

            if (target is GameObject targetGObj)
            {
                Debug.Log(targetGObj.name);
            }
        }
    }
}
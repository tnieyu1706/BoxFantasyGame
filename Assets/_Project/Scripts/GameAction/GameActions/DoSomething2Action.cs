using System;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using UnityEngine;

namespace Systems.GameAction.GameActions
{
    [ActionIdentifyStorageFlag("DoSomething2")]
    [Serializable]
    public class DoSomething2Action : GameAction<TestObjectIdentify, TestObjectIdentify>
    {
        public override void Execute(TestObjectIdentify sender, TestObjectIdentify target)
        {
            Debug.Log("You are doing something 2.");
        }
    }
}
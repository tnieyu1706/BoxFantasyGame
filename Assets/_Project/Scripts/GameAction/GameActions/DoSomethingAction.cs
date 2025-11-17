using System;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using UnityEngine;

namespace Systems.GameAction.GameActions
{
    [ActionIdentifyStorageFlag("DoSomething")]
    [Serializable]
    public class DoSomethingAction : GameAction<TestObjectIdentify, TestObjectIdentify>
    {
        public override void Execute(TestObjectIdentify sender, TestObjectIdentify target)
        {
            Debug.Log("You are doing something not special.");
        }
    }
}
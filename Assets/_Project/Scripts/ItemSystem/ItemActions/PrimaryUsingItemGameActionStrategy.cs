using System;
using System.Collections.Generic;
using Systems.GameAction;

namespace Systems.ItemSystem.ItemActions
{
    public class PrimaryUsingItemGameActionStrategy : IGameActionStrategy
    {
        public const string ACTION_IDENTIFY_NAME = "PrimaryUsingItemAction";
        public string ActionIdentifyName => ACTION_IDENTIFY_NAME;

        public void Execute(object payload, object sender, object target)
        {
            if (payload is not ItemData itemPayload) return;
            if (sender is not IItemUser itemUser) return;

            itemPayload.primaryUsingConfig.usingProcedure
                .HandleProcedure(itemUser, target, itemPayload.primaryUsingConfig.usingEffects);
        }
    }
}
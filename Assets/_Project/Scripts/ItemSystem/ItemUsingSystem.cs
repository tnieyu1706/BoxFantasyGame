using Systems.GameAction;
using Systems.ItemSystem.ItemActions;

namespace Systems.ItemSystem
{
    public static class ItemUsingSystem
    {
        private static PrimaryUsingItemGameActionStrategy primaryUsingItemGameActionStrategy;

        private static PrimaryUsingItemGameActionStrategy PrimaryUsingItemGameActionStrategy =>
            primaryUsingItemGameActionStrategy ??= new PrimaryUsingItemGameActionStrategy();

        public static void UseItemPrimary(IItemUser itemUser, ItemData itemData)
        {
            var primaryUsingConfig = itemData.primaryUsingConfig;
            object targetTracking = primaryUsingConfig.usingTargetTracking.TrackTarget(itemUser);

            if (primaryUsingConfig.isNeedTarget && targetTracking is null)
                return;

            var usingItemActionCommand =
                new GameActionCommand(
                    itemUser,
                    targetTracking,
                    PrimaryUsingItemGameActionStrategy,
                    itemData
                );
            GameActionSystem.Instance.PushCommand(usingItemActionCommand);
        }
    }
}
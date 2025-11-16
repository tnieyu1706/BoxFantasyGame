using System;
using System.Reflection;
using Systems.IdentifySystem.ObjectIdentify;
using Systems.IdentifySystem.TypeIdentify;

namespace Systems.GameAction
{
    /// <summary>
    /// Required own _IdentifyStorageFlag to tracking & manager.
    /// </summary>
    public interface IGameAction : IIdentifyFlagSupport
    {
        void Execute(IObjectIdentify sender, IObjectIdentify target);
    }

    // public static class InterfaceGameActionExtensions
    // {
    //     public static string GetActionIdentify(this IGameAction gameAction)
    //     {
    //         var attr = gameAction.GetType()
    //             .GetCustomAttribute(typeof(ActionIdentifyStorageFlagAttribute));
    //
    //         if (attr != null && attr is ActionIdentifyStorageFlagAttribute actionFlagAttr)
    //         {
    //             return actionFlagAttr.Identify;
    //         }
    //
    //         return null;
    //     }
    // }

    [ActionIdentifyStorageFlag("_Action")]
    [Serializable]
    public abstract class GameAction<TSender, TTarget> : IGameAction
        where TSender : IObjectIdentify
        where TTarget : IObjectIdentify
    {
        public void Execute(IObjectIdentify sender, IObjectIdentify target)
        {
            Execute((TSender)sender, (TTarget)target);
        }

        public abstract void Execute(TSender sender, TTarget target);
    }
    
}
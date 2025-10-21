using System;
using Systems.Identify;

namespace Systems.GameAction
{
    /// <summary>
    /// Required own _IdentifyStorageFlag to tracking & manager.
    /// </summary>
    public interface IGameAction : IIdentifyFlagSupport
    {
        void Execute(IGameActionSubject sender, IGameActionSubject target);
    }

    [ActionIdentifyStorageFlag("_Action")]
    [Serializable]
    public abstract class GameAction<TSender, TTarget> : IGameAction
        where TSender : IGameActionSubject
        where TTarget : IGameActionSubject
    {
        public void Execute(IGameActionSubject sender, IGameActionSubject target)
        {
            Execute((TSender)sender, (TTarget)target);
        }

        public abstract void Execute(TSender sender, TTarget target);
    }
    
}
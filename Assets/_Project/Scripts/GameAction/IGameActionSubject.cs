using System;
using Systems.Identify;

namespace Systems.GameAction
{
    /// <summary>
    /// Required own _IdentifyStorageFlag to tracking & manager.
    /// </summary>
    public interface IGameActionSubject : IIdentifyFlagSupport
    {
        
    }

    [SubjectIdentifyStorageFlag("_Subject")]
    [Serializable]
    public abstract class GameActionSubject<T> : IGameActionSubject
    {
        public T data;
    }
}
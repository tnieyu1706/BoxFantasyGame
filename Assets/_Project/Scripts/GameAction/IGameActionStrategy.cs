namespace Systems.GameAction
{
    /// <summary>
    /// Required own _IdentifyStorageFlag to tracking & manager.
    /// </summary>
    public interface IGameActionStrategy
    {
        string ActionIdentifyName { get; }

        void Execute(object payload, object sender, object target);
    }
}
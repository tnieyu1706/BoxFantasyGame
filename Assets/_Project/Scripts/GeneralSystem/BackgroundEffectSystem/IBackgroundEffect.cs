namespace Systems.GeneralSystem.BackgroundEffectSystem
{
    public interface IBackgroundEffect
    {
        void Perform();
    }

    public interface ITriggerBackgroundEffect : IBackgroundEffect
    {
        bool IsTriggered { get; set; }
    }
}
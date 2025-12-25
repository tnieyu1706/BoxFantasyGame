namespace Systems.ItemSystem
{
    public interface IUsingEffect
    {
        void Perform(object target);
    }

    public interface ITemporaryUsingEffect<in T> : IUsingEffect
    {
        float Duration { get;}
    }
}
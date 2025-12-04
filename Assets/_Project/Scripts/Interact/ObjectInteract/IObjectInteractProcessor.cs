namespace Systems.Interact.ObjectInteract
{
    public interface IObjectInteractProcessor
    {
        void SubscribeEvent(ObjectInteractor visitor);
        void UnsubscribeEvent(ObjectInteractor visitor);
    }
}
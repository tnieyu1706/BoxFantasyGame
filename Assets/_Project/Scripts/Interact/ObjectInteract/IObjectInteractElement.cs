using TnieYuPackage.DesignPatterns.Patterns.Visitor;

namespace Systems.Interact.ObjectInteract
{
    public interface IObjectInteractElement : IElement<IObjectInteractVisitor>
    {
        void IElement<IObjectInteractVisitor>.Accept(IObjectInteractVisitor visitor)
        {
            
        }
        
        void SubscribeEvent(IObjectInteractVisitor visitor);
        void UnsubscribeEvent(IObjectInteractVisitor visitor);
    }
}
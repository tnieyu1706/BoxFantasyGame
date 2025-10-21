using TnieYuPackage.DesignPatterns.Patterns.Visitor;

namespace Systems.Interact
{
    public interface IInteractElement : IElement<IInteractVisitor>
    {
        void IElement<IInteractVisitor>.Accept(IInteractVisitor visitor)
        {
            
        }
        
        void SubscribeEvent(IInteractVisitor visitor);
        void UnsubscribeEvent(IInteractVisitor visitor);
    }
}
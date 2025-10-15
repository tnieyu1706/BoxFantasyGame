using TnieYuPackage.DesignPatterns.Patterns.Visitor;

namespace Systems.Interact
{
    public interface IInteractElement : IElement<IInteractVisitor>
    {
        
    }
}
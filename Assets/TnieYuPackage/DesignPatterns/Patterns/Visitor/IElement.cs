namespace TnieYuPackage.DesignPatterns.Patterns.Visitor
{
    public interface IElement
    {
        void Accept(IVisitor visitor);
    }

    public interface IElement<TVisitor> : IElement
        where TVisitor : IVisitor
    {
        void IElement.Accept(IVisitor visitor) => Accept((TVisitor)visitor);
        void Accept(TVisitor visitor);
    }
}
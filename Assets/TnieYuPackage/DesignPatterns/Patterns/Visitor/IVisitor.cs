namespace TnieYuPackage.DesignPatterns.Patterns.Visitor
{
    public interface IVisitor
    {
        void Visit(IElement element);
    }

    public interface IVisitor<TElement> : IVisitor
        where TElement : IElement
    {
        void IVisitor.Visit(IElement element) => Visit((TElement)element);
        void Visit(TElement element);
    }
}
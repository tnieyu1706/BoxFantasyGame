using TnieCustomPackage.SerializeInterface;
using TnieYuPackage.DesignPatterns.Patterns.Visitor;
using UnityEngine;

namespace Systems.PropertyDataSystem.VisitorPattern
{
    public abstract class BasePropertyVisitor<T> : MonoBehaviour, IVisitor<IElement>
        where T : class
    {
        public InterfaceReference<T> property;
        public virtual void Visit(IElement element)
        {
            element.Accept(this);
        }
    }
}
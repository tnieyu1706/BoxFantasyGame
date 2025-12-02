using System;
using TnieYuPackage.DesignPatterns.Patterns.Visitor;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Interact.ObjectInteract
{
    public interface IObjectInteractVisitor : IVisitor<IObjectInteractElement>
    {
        void IVisitor<IObjectInteractElement>.Visit(IObjectInteractElement element)
        {
            
        }
        UnityEvent @Event { get; }
    }

    [Serializable]
    public class ObjectInteractVisitorConcrete : IObjectInteractVisitor
    {
        [SerializeField] private UnityEvent @event = new();
        public UnityEvent Event => @event;
    }
}
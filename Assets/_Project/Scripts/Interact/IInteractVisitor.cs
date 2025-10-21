using System;
using TnieYuPackage.DesignPatterns.Patterns.Visitor;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Interact
{
    public interface IInteractVisitor : IVisitor<IInteractElement>
    {
        void IVisitor<IInteractElement>.Visit(IInteractElement element)
        {
            
        }
        UnityEvent @Event { get; }
    }

    [Serializable]
    public class InteractVisitorConcrete : IInteractVisitor
    {
        [SerializeField] private UnityEvent @event = new();
        public UnityEvent Event => @event;
    }
}
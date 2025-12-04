using System;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Interact.ObjectInteract
{
    [Serializable]
    public class ObjectInteractor
    {
        [SerializeField] private UnityEvent @event = new();
        public UnityEvent Event => @event;
    }
}
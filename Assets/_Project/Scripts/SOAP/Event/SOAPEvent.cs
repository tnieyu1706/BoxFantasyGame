using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.SOAP.Event
{
    [CreateAssetMenu(fileName = "SOAPVoidEvent", menuName = "Scriptable Objects/SOAP/Event/Void")]
    public class SOAPEvent : ScriptableObject
    {
        public UnityEvent @event;
        
        [Button]
        public void Raise() => @event?.Invoke();
    }

    public abstract class SOAPEventGeneric<T> : ScriptableObject
    {
        public UnityEvent<T> @event;
        
        [Button]
        public void Raise(T value) => @event?.Invoke(value);
    }
}
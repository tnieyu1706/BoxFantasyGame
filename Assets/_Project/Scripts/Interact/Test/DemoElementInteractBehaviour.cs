using UnityEngine;

namespace Systems.Interact.Test
{
    public class DemoElementInteractBehaviour : MonoBehaviour, IInteractElement
    {
        public void SubscribeEvent(IInteractVisitor visitor)
        {
            visitor.Event.AddListener(DebugLog);
        }

        public void UnsubscribeEvent(IInteractVisitor visitor)
        {
            visitor.Event.RemoveListener(DebugLog);
        }

        private void DebugLog()
        {
            Debug.Log($"ElementInteractBehaviour_{gameObject.name}");
        }
    }
}
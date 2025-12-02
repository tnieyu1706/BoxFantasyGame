using UnityEngine;

namespace Systems.Interact.ObjectInteract.Test
{
    public class DemoElementObjectInteractBehaviour : MonoBehaviour, IObjectInteractElement
    {
        public void SubscribeEvent(IObjectInteractVisitor visitor)
        {
            visitor.Event.AddListener(DebugLog);
        }

        public void UnsubscribeEvent(IObjectInteractVisitor visitor)
        {
            visitor.Event.RemoveListener(DebugLog);
        }

        private void DebugLog()
        {
            Debug.Log($"ElementInteractBehaviour_{gameObject.name}");
        }
    }
}
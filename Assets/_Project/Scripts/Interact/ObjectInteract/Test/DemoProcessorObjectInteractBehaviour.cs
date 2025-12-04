using UnityEngine;

namespace Systems.Interact.ObjectInteract.Test
{
    public class DemoProcessorObjectInteractBehaviour : MonoBehaviour, IObjectInteractProcessor
    {
        public void SubscribeEvent(ObjectInteractor visitor)
        {
            visitor.Event.AddListener(DebugLog);
        }

        public void UnsubscribeEvent(ObjectInteractor visitor)
        {
            visitor.Event.RemoveListener(DebugLog);
        }

        private void DebugLog()
        {
            Debug.Log($"ElementInteractBehaviour_{gameObject.name}");
        }
    }
}
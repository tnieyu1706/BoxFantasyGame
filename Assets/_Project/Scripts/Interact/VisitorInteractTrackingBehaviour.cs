using Systems.Input;
using UnityEngine;

namespace Systems.Interact
{
    public class VisitorInteractTrackingBehaviour : MonoBehaviour
    {
        [SerializeReference] public IInteractVisitor visitor = new InteractVisitorConcrete();

        void OnEnable()
        {
            ScreenInteractTracking.Instance.OnTrackingEnter += TrackingEnter;
            ScreenInteractTracking.Instance.OnTrackingExit += TrackingExit;
            PlayerInputReader.Instance. Interact+= visitor.Event.Invoke;
        }

        void OnDisable()
        {
            if (ScreenInteractTracking.Instance != null)
            {
                ScreenInteractTracking.Instance.OnTrackingEnter -= TrackingEnter;
                ScreenInteractTracking.Instance.OnTrackingExit -= TrackingExit;
            }

            if (PlayerInputReader.Instance != null)
            {
                PlayerInputReader.Instance.Interact -= visitor.Event.Invoke;
            }
        }

        void TrackingEnter(Collider target)
        {
            IInteractElement element = target.GetComponent<IInteractElement>();
            if (element == null)
            {
                Debug.Log($"{target.name} dont contains IInteractElement");
                return;
            }

            element.SubscribeEvent(visitor);
        }

        void TrackingExit(Collider target)
        {
            IInteractElement element = target.GetComponent<IInteractElement>();
            if (element == null)
            {
                Debug.Log($"{target.name} dont contains IInteractElement");
                return;
            }

            element.UnsubscribeEvent(visitor);
        }
    }
}
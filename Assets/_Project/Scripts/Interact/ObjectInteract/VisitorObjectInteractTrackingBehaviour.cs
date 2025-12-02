using Systems.Input;
using UnityEngine;

namespace Systems.Interact.ObjectInteract
{
    public class VisitorObjectInteractTrackingBehaviour : MonoBehaviour
    {
        [SerializeReference] private IObjectInteractVisitor visitor;

        public IObjectInteractVisitor Visitor
        {
            get
            {
                if (visitor == null)
                    visitor = new ObjectInteractVisitorConcrete();
                return visitor;
            }
        }

        void OnEnable()
        {
            ScreenObjectInteractTracking.Instance.OnTrackingEnter += TrackingEnter;
            ScreenObjectInteractTracking.Instance.OnTrackingExit += TrackingExit;
            PlayerInputReader.Instance.interact.Event += Visitor.Event.Invoke;
        }

        void OnDisable()
        {
            if (ScreenObjectInteractTracking.Instance != null)
            {
                ScreenObjectInteractTracking.Instance.OnTrackingEnter -= TrackingEnter;
                ScreenObjectInteractTracking.Instance.OnTrackingExit -= TrackingExit;
            }

            if (PlayerInputReader.Instance != null)
            {
                PlayerInputReader.Instance.interact.Event -= Visitor.Event.Invoke;
            }
        }

        void TrackingEnter(Collider target)
        {
            IObjectInteractElement element = target.GetComponent<IObjectInteractElement>();
            if (element == null)
            {
                Debug.Log($"{target.name} dont contains IInteractElement");
                return;
            }

            element.SubscribeEvent(Visitor);

            InputUIManager.Instance.ShowInput(PlayerInputReader.Instance.interact.InputName);
        }

        void TrackingExit(Collider target)
        {
            IObjectInteractElement element = target.GetComponent<IObjectInteractElement>();
            if (element == null)
            {
                Debug.Log($"{target.name} dont contains IInteractElement");
                return;
            }

            element.UnsubscribeEvent(Visitor);

            InputUIManager.Instance.HideInput(PlayerInputReader.Instance.interact.InputName);
        }
    }
}
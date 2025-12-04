using Systems.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Interact.ObjectInteract
{
    public class VisitorObjectInteractTrackingBehaviour : MonoBehaviour
    {
        [SerializeField] private ObjectInteractor interactor = new();

        void OnEnable()
        {
            ScreenObjectInteractTracking.Instance.OnTrackingEnter += TrackingEnter;
            ScreenObjectInteractTracking.Instance.OnTrackingExit += TrackingExit;
            PlayerInputReader.Instance.interact.Event += interactor.Event.Invoke;
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
                PlayerInputReader.Instance.interact.Event -= interactor.Event.Invoke;
            }
        }

        void TrackingEnter(Collider target)
        {
            IObjectInteractProcessor processor = target.GetComponent<IObjectInteractProcessor>();
            if (processor == null)
            {
                Debug.Log($"{target.name} dont contains IInteractElement");
                return;
            }

            processor.SubscribeEvent(interactor);

            InputUIManager.Instance.ShowInput(PlayerInputReader.Instance.interact.InputName);
        }

        void TrackingExit(Collider target)
        {
            IObjectInteractProcessor processor = target.GetComponent<IObjectInteractProcessor>();
            if (processor == null)
            {
                Debug.Log($"{target.name} dont contains IInteractElement");
                return;
            }

            processor.UnsubscribeEvent(interactor);

            InputUIManager.Instance.HideInput(PlayerInputReader.Instance.interact.InputName);
        }
    }
}
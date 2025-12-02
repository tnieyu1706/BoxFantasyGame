using EditorAttributes;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.GlobalExtensions;
using TnieYuPackage.SOAP.Event;
using UnityEngine;

namespace Systems.CollisionSystem
{
    public abstract class BaseInteractTracking : MonoBehaviour
    {
        [TnieLayerMaskDropdown] [SerializeField]
        protected int interactLayerMask;

        [SerializeField, Required] protected ColliderSoapEvent2So onInteractEnter;
        [SerializeField, Required] protected Collider subjectCollider;

        protected virtual void OnInteractEnter(Collider target)
        {
            if (interactLayerMask.ContainLayer(target.gameObject.layer))
            {
                onInteractEnter.Event?.Invoke(subjectCollider, target);
            }
        }
    }

    public class TriggerInteractTracking : BaseInteractTracking
    {
        protected void OnTriggerEnter(Collider other)
        {
            OnInteractEnter(other);
        }
    }
}
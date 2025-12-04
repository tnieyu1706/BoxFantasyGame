using TnieYuPackage.CustomAttributes;
using TnieYuPackage.GlobalExtensions;
using UnityEngine;

namespace Systems.PropertyDataSystem.UnitHandlers
{
    public abstract class UnitInteractor : MonoBehaviour
    {
        [SerializeField] [TnieLayerMaskDropdown]
        private int interactionLayerMask;

        private void OnTriggerEnter(Collider other)
        {
            if (interactionLayerMask.ContainLayer(other.gameObject.layer))
                OnInteract(other);
        }

        protected abstract void OnInteract(Collider other);
    }
}
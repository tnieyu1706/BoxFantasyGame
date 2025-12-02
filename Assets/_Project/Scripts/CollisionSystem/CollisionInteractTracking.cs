using TnieYuPackage.GlobalExtensions;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace Systems.CollisionSystem
{
    public class CollisionInteractTracking : BaseInteractTracking
    {
        private void OnCollisionEnter(Collision collision)
        {
            OnInteractEnter(collision.collider);
        }
    }
}
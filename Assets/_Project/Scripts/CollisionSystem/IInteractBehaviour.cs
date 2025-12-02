using TnieYuPackage.SOAP.Event;
using UnityEngine;

namespace Systems.CollisionSystem
{
    public interface IInteractEnterBehaviour
    {
        ColliderSoapEvent2So OnEnter { get; set; }

        void OnGetEntered(Collider sender, Collider target);
    }

    public interface IInteractExitBehaviour
    {
        ColliderSoapEvent2So OnExit { get; set; }

        void OnGetExited(Collider sender, Collider target);
    }
}
using Systems.CollisionSystem;
using UnityEngine;

namespace Systems.EntityDataSystem.Monster
{
    public abstract class BaseMonsterInteractBehaviour : MonoBehaviour
    {
        protected void OnEnable()
        {
            if (this is IInteractEnterBehaviour enterBehaviour)
            {
                enterBehaviour.OnEnter.Event += enterBehaviour.OnGetEntered;
            }

            // if (this is IInteractExitBehaviour exitBehaviour)
            // {
            //     exitBehaviour.OnExit.Event += exitBehaviour.OnGetExited;
            // }
        }

        protected void OnDisable()
        {
            if (this is IInteractEnterBehaviour enterBehaviour)
            {
                enterBehaviour.OnEnter.Event -= enterBehaviour.OnGetEntered;
            }

            // if (this is IInteractExitBehaviour exitBehaviour)
            // {
            //     exitBehaviour.OnExit.Event -= exitBehaviour.OnGetExited;
            // }
        }
    }
}
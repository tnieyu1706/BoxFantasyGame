using Systems.EntityDataSystem;
using Systems.EntityDataSystem.Player;
using UnityEngine;

namespace _Project.Test.Entity
{
    public class TestPlayerGetAttackedByMonster : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IEntityData monsterBehaviour = other.GetComponent<IEntityData>();
            if (monsterBehaviour != null)
            {
                PlayerEntityData.Instance.Health -= monsterBehaviour.AttackedDamage;
                
                Debug.Log(monsterBehaviour.AttackedDamage);
            }
        }
    }
}
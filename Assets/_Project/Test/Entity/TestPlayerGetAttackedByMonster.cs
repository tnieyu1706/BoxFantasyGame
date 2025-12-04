using Systems.EntityDataSystem;
using Systems.EntityDataSystem.Player;
using UnityEngine;

namespace _Project.Test.Entity
{
    public class TestPlayerGetAttackedByMonster : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IEntity monsterBehaviour = other.GetComponent<IEntity>();
            if (monsterBehaviour != null)
            {
                PlayerEntity.Instance.CurrentHealth -= monsterBehaviour.AttackedDamage;
                
                Debug.Log(monsterBehaviour.AttackedDamage);
            }
        }
    }
}
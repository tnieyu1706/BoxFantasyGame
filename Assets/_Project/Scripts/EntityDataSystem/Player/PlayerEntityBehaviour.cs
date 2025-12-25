using UnityEngine;

namespace Systems.EntityDataSystem.Player
{
    public class PlayerEntityBehaviour : MonoBehaviour, IEntity
    {
        [SerializeField] private PlayerEntity playerEntity;

        public int CurrentHealth
        {
            get => playerEntity.CurrentHealth;
            set => playerEntity.CurrentHealth = value;
        }

        public int MaxHealth
        {
            get => playerEntity.MaxHealth;
            set => playerEntity.MaxHealth = value;  
        }
        public int AttackedDamage => playerEntity.AttackedDamage;
        public float MoveSpeed => playerEntity.MoveSpeed;
    }
}
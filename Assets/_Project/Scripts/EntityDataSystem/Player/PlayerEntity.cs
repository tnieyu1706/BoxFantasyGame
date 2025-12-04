using System;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using TnieYuPackage.SOAP.Data;
using UnityEngine;

namespace Systems.EntityDataSystem.Player
{
    [CreateAssetMenu(fileName = "PlayerEntity", menuName = "Scriptable Objects/EntityData/Player")]
    public sealed class PlayerEntity : StaticObjectSingletonIdentify<PlayerEntity>, IEntity
    {
        [SerializeField] private SoapData<int> health;
        [SerializeField] private int maxHealth;
        [SerializeField] private int attackedDamage;
        [SerializeField] private float moveSpeed;

        public int CurrentHealth
        {
            get => health.Value;
            set => health.Value = value;
        }

        public Action<int> OnHealthChanged
        {
            get => health.OnValueChange;
            set => health.OnValueChange = value;
        }

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int AttackedDamage => attackedDamage;
        public float MoveSpeed => moveSpeed;
        
    }
    
}
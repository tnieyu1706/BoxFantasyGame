using System;
using EditorAttributes;
using Systems.PropertyDataSystem.Properties;
using UnityEngine;

namespace Systems.EntityDataSystem.Monster
{
    public class MonsterEntityBehaviour : MonoBehaviour, IEntityData
    {
        #region STATIC PROPERTIES

        //current -> only apply to (constant) value: (causerDamage, dealingDamage)
        public static Action<int> onMonsterApplyHealth;
        public static Action<int> onMonsterDealDamage;
        
        #endregion
        
        #region BASE PROPERTIES
        
        [SerializeField, Required] private MonsterEntityDataPresetSo monsterDataPreset;
        [SerializeField] private int currentHealth;

        public int Health
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public int AttackedDamage { get; set; }
        public float MoveSpeed { get; set; }

        [Button]
        public void LoadPreset()
        {
            Health = monsterDataPreset.health;
            AttackedDamage = monsterDataPreset.attackedDamage;
            MoveSpeed = monsterDataPreset.moveSpeed;
        }

        //test 
        public void Start()
        {
            LoadPreset();
        }
        
        #endregion
        
        #region METHODS

        public void ApplyDamageBySubject(IAttackedDamageProperty subjectDamage)
        {
            this.Health -= subjectDamage.AttackedDamage;
            onMonsterApplyHealth?.Invoke(subjectDamage.AttackedDamage);
        }

        public void DealDamageToTarget(IHealthProperty targetHealth)
        {
            targetHealth.Health -= AttackedDamage;
            onMonsterDealDamage?.Invoke(AttackedDamage);
        }
        
        #endregion
    }
}
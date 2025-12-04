using EditorAttributes;
using UnityEngine;

namespace Systems.EntityDataSystem.Monster
{
    public class MonsterEntity : MonoBehaviour, IEntity
    {
        #region BASE PROPERTIES
        
        [SerializeField, Required] private MonsterEntityPresetSo monsterPreset;

        [field: SerializeField]
        public int CurrentHealth { get; set; }
        
        [field: SerializeField]
        public int MaxHealth { get; set; }

        public int AttackedDamage => monsterPreset.attackedDamage;
        public float MoveSpeed => monsterPreset.moveSpeed;

        //test 
        
        [Button]
        public void LoadPresetManual()
        {
            monsterPreset.ApplyPreset(this);
        }
        
        public void Start()
        {
            monsterPreset.ApplyPreset(this);
        }
        
        #endregion
    }
}
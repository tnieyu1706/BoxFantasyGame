using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using UnityEngine;

namespace Systems.EntityDataSystem.Monster
{
    [CreateAssetMenu(fileName = "MonsterEntityPreset", menuName = "Scriptable Objects/EntityData/Monster")]
    public class MonsterEntityPresetSo : StaticObjectIdentify
    {
        public int maxHealth;
        public int attackedDamage;
        public float moveSpeed;

        public void ApplyPreset(IEntity entity)
        {
            entity.MaxHealth = maxHealth;
            entity.CurrentHealth = entity.MaxHealth;
        }
    }
}
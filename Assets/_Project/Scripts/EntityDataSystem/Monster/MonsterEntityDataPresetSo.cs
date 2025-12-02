using System;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using UnityEngine;

namespace Systems.EntityDataSystem.Monster
{
    [CreateAssetMenu(fileName = "MonsterEntityData", menuName = "Scriptable Objects/EntityData/Monster")]
    public class MonsterEntityDataPresetSo : StaticObjectIdentify
    {
        public int health;
        public int attackedDamage;
        public float moveSpeed;
    }
}
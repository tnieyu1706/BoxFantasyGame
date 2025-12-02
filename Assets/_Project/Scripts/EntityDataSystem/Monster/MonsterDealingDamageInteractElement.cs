using EditorAttributes;
using Systems.PropertyDataSystem.VisitorPattern;
using TnieYuPackage.DesignPatterns.Patterns.Visitor;
using UnityEngine;

namespace Systems.EntityDataSystem.Monster
{
    public abstract class BaseMonsterInteractElement : MonoBehaviour
    {
        [SerializeField] [Required]
        protected MonsterEntityBehaviour monsterBehaviour;
    }
    
    public class MonsterDealingDamageInteractElement : BaseMonsterInteractElement, IElement<HealthPropertyVisitor>
    {
        public void Accept(HealthPropertyVisitor visitor)
        {
            monsterBehaviour.DealDamageToTarget(visitor.property.Value);
        }
    }
    
    public class MonsterApplyDamageInteractElement : BaseMonsterInteractElement, IElement<AttackedDamagePropertyVisitor>
    {
        public void Accept(AttackedDamagePropertyVisitor visitor)
        {
            monsterBehaviour.ApplyDamageBySubject(visitor.property.Value);
        }
    }
}
using Systems.CollisionSystem;
using Systems.PropertyDataSystem.VisitorPattern;
using TnieCustomPackage.BackboneLogger;
using TnieYuPackage.SOAP.Event;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace Systems.EntityDataSystem.Monster
{
    public class MonsterDealingDamageInteractBehaviour : BaseMonsterInteractBehaviour, IInteractEnterBehaviour
    {
        [SerializeField] private ColliderSoapEvent2So onEnterEvent;

        public ColliderSoapEvent2So OnEnter
        {
            get => onEnterEvent;
            set => onEnterEvent = value;
        }
        public void OnGetEntered(Collider sender, Collider monster)
        {
            MonsterDealingDamageInteractElement monsterDealingDamageElement = monster.GetComponent<MonsterDealingDamageInteractElement>();
            if (monsterDealingDamageElement == null)
            {
                Logger.Log($"Monster {monster.name} don't have component MonsterApplyDamageInteractElement", LogLevel.Warning, "Debug");
                return;
            }
            
            HealthPropertyVisitor entityVisitor = sender.GetComponent<HealthPropertyVisitor>();
            if (entityVisitor == null)
            {
                Logger.Log($"Sender {sender.name} don't have component EntityInteractVisitor", LogLevel.Warning, "Debug");
                return;
            }
            
            Logger.Log($"Sender {sender.name} visit monster {monster.name}");
            entityVisitor.Visit(monsterDealingDamageElement);
        }
    }
}
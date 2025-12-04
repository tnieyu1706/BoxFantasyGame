using Systems.PropertyDataSystem.Properties;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace Systems.PropertyDataSystem.UnitHandlers
{
    public class UnitGettingDamageInteractor : UnitInteractor
    {
        [SerializeField] private InterfaceReference<IHealthProperty> entityHealth;

        protected override void OnInteract(Collider other)
        {
            UnitDealingDamageProcessor dealingDamageProcessor = other.GetComponent<UnitDealingDamageProcessor>();

            if (dealingDamageProcessor == null)
            {
                Logger.Log(
                    $"Collision object [{other.gameObject.name}] not found component EntityDealingDamageProcessor.");
                return;
            }

            dealingDamageProcessor.Process(entityHealth.Value);
        }
    }
}
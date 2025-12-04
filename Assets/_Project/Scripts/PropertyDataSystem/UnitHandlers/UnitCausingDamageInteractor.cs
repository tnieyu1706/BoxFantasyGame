using Systems.PropertyDataSystem.Properties;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace Systems.PropertyDataSystem.UnitHandlers
{
    public class UnitCausingDamageInteractor : UnitInteractor
    {
        [SerializeField] private InterfaceReference<IAttackedDamageProperty> entityAttack;

        protected override void OnInteract(Collider other)
        {
            UnitApplyDamageProcessor applyDamageProcessor = other.GetComponent<UnitApplyDamageProcessor>();

            if (applyDamageProcessor == null)
            {
                Logger.Log(
                    $"Collision object [{other.gameObject.name}] not found component EntityApplyDamageProcessor.");
                return;
            }

            applyDamageProcessor.Process(entityAttack.Value);
        }
    }
}
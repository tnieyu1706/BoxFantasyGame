using Systems.PropertyDataSystem.Properties;
using TnieCustomPackage.SerializeInterface;
using TnieYuPackage.DesignPatterns.FunctionalInterfaces;
using UnityEngine;

namespace Systems.PropertyDataSystem.UnitHandlers
{
    public class UnitApplyDamageProcessor : MonoBehaviour, IProcessor<IAttackedDamageProperty>
    {
        [SerializeField] private InterfaceReference<IHealthProperty> entityHealth;

        public void Process(IAttackedDamageProperty individual)
        {
            entityHealth.Value.CurrentHealth -= individual.AttackedDamage;
        }
    }
}
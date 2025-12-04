using Systems.PropertyDataSystem.Properties;
using TnieCustomPackage.SerializeInterface;
using TnieYuPackage.DesignPatterns.FunctionalInterfaces;
using UnityEngine;

namespace Systems.PropertyDataSystem.UnitHandlers
{
    public class UnitDealingDamageProcessor : MonoBehaviour, IProcessor<IHealthProperty>
    {
        [SerializeField] private InterfaceReference<IAttackedDamageProperty> entityAttack;

        public void Process(IHealthProperty individual)
        {
            individual.CurrentHealth -= entityAttack.Value.AttackedDamage;
        }
    }
}
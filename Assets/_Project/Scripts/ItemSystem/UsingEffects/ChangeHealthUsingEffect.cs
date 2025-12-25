using System;
using Systems.PropertyDataSystem.Properties;
using TnieCustomPackage.BackboneLogger;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace Systems.ItemSystem.UsingEffects
{
    [Serializable]
    public class ChangeHealthUsingEffect : IUsingEffect
    {
        [SerializeField] private int changedHealth;
        
        public void Perform(object target)
        {
            if (target is GameObject targetGameObject)
            {
                var healthProperty = targetGameObject.GetComponent<IHealthProperty>();

                if (healthProperty == null)
                {
                    Logger.Log($"target {targetGameObject.name} has no health property.", LogLevel.Debug, "Runtime");
                    return;
                }

                healthProperty.CurrentHealth += changedHealth;
                Logger.Log($"completed changed Health for {targetGameObject.name} entity.", LogLevel.Debug, "Runtime");
            }
        }
    }
}
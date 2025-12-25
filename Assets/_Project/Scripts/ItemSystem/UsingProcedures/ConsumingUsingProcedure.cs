using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.ItemSystem.UsingProcedures
{
    [Serializable]
    public class ConsumingUsingProcedure : IUsingProcedure
    {
        public void HandleProcedure(IItemUser itemUser, object target, List<IUsingEffect> effects)
        {
            Debug.Log("IUsingProcedure.HandleProcedure");

            foreach (var effect in effects)
            {
                effect.Perform(target);
            }
        }
    }
}
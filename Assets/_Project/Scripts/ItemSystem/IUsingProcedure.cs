using System;
using System.Collections.Generic;
using Systems.EntityDataSystem;
using UnityEngine;

namespace Systems.ItemSystem
{
    /// <summary>
    /// IUsingProcedure is interface apply for ItemSystem
    /// with target => specify procedural working of item when using.
    /// And Current it only work with {Entity}
    /// </summary>
    public interface IUsingProcedure
    {
        void HandleProcedure(IEntity entity, List<IUsingEffect> effects);
    }
    
    //IUsingProcedure implements
    
    [Serializable]
    public class ConsumingUsingProcedure : IUsingProcedure
    {
        public void HandleProcedure(IEntity entity, List<IUsingEffect> effects)
        {
            Debug.Log("IUsingProcedure.HandleProcedure");

            foreach (var effect in effects)
            {
                effect.Perform(entity);
            }
        }
    }
}
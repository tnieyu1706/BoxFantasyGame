using System;
using Systems.PropertyDataSystem.Properties;
using UnityEngine;

namespace Systems.ItemSystem
{
    public interface IUsingEffect
    {
        void Perform(object target);
        bool IsValidUsing(object target);
    }

    public interface IUsingEffect<in T> : IUsingEffect
    {
        void IUsingEffect.Perform(object target)
        {
            if (!IsValidUsing(target)) return;
            
            Perform((T)target);
        }

        void Perform(T target);

        bool IUsingEffect.IsValidUsing(object target) => target is T;
    }

    public interface ITemporaryUsingEffect<in T> : IUsingEffect<T>
    {
        float Duration { get;}
    }
    
    //IUsingEffect implements

    [Serializable]
    public class ChangeHealthUsingEffect : IUsingEffect<IHealthProperty>
    {
        [SerializeField] private int changedHealth;

        public void Perform(IHealthProperty target)
        {
            target.CurrentHealth += changedHealth;
        }
    }
}
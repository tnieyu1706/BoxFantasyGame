using System;
using Systems.GameAction;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.ObjectDataSystem;
using UnityEngine;

namespace Systems.GeneralSystem.RequirementSystem
{
    public interface IRequirement
    {
        bool CheckedValue { get; set; }
        object ValidatedData { get; }
        void OnComplete();

        bool Validate(object data);
    }


    [Serializable]
    public abstract class FieldRequirement : IRequirement
    {
        public string keyName;
        public bool checkedValue;

        [SerializeReference] [AbstractSupport(typeof(IObjectData))]
        public IObjectData validatedData;

        public bool CheckedValue
        {
            set => checkedValue = value;
            get => checkedValue;
        }

        public object ValidatedData => validatedData;
        public abstract void OnComplete();
        public abstract bool Validate(object data);
    }

    [Serializable]
    public abstract class ActionRequirement : IRequirement
    {
        [SerializeField] private bool checkedValue;

        [SerializeReference] [AbstractSupport(typeof(IGameActionCatcher))]
        private IGameActionCatcher catcherValidatedData;

        private IRequirement requirementImplementation;

        public bool CheckedValue
        {
            get => checkedValue;
            set => checkedValue = value;
        }

        public object ValidatedData => catcherValidatedData;
        public abstract void OnComplete();
        
        public abstract bool Validate(object data);
    }
}
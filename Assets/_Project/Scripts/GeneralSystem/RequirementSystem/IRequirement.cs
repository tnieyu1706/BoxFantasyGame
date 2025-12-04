using System;
using EditorAttributes;
using Systems.GameAction;
using Systems.GameAction.Editor;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.ObjectDataSystem;
using TnieYuPackage.Utils;
using UnityEngine;

namespace Systems.GeneralSystem.RequirementSystem
{
    public interface IRequirement
    {
        object ValidatedData { get; }
        bool CheckedValue { get; set; }
        string KeyName { get; }
        void CompleteResult();
    }

    public interface IRequirement<TPayload> : IRequirement
    {
        object IRequirement.ValidatedData => ActualValidatedData;
        TPayload ActualValidatedData { get; }

        bool Validate(TPayload payload);

        public bool LoadRequirement(TPayload payload)
        {
            if (Validate(payload))
            {
                CheckedValue = true;
                CompleteResult();
                //unregistry in triggeraction by return to true
                return true;
            }

            return false;
        }
    }

    public static class InterfaceRequirementExtensions
    {
        public static bool LoadRequirement<T>(this IRequirement<T> requirement, T payload)
        {
            return requirement.LoadRequirement(payload);
        }

        public static bool Subscribe<T>(this IRequirement<T> requirement, BaseTriggerAction<T> triggerAction)
        {
            if (triggerAction == null)
                return false;

            return triggerAction.TryToAddTrigger(requirement.LoadRequirement);
        }

        public static void UnSubscribe<T>(this IRequirement<T> requirement, BaseTriggerAction<T> triggerAction)
        {
            triggerAction.RemoveTrigger(requirement.LoadRequirement);
        }
    }

    [Serializable]
    public abstract class BaseRequirement<TPayload> : IRequirement<TPayload>
    {
        #region Fields

        [SerializeField] private bool checkedValue;

        [SerializeReference, AbstractSupport()]
        private TPayload validatedData;

        public TPayload ActualValidatedData => validatedData;

        #endregion

        public bool CheckedValue
        {
            get => checkedValue;
            set => checkedValue = value;
        }

        public abstract string KeyName { get; }
        public abstract void CompleteResult();
        public abstract bool Validate(TPayload payload);
    }

    [Serializable]
    public abstract class FieldRequirement : BaseRequirement<IObjectData>
    {
        [PropertyOrder(-1)] [SerializeField] private string keyName;
        public override string KeyName => keyName;
    }

    [Serializable]
    public abstract class ActionRequirement : BaseRequirement<GameActionCommandStaticDto>
    {
        [PropertyOrder(-1)] [SerializeField, SelectAction]
        private string keyName;

        public override string KeyName => keyName;

        public void SubscribeRequirement()
        {
            if (CheckedValue || string.IsNullOrEmpty(KeyName)) return;

            if (ActionIdentifyStorage.Instance.TryToGetLoadEvent(KeyName, out var triggerAction) &&
                this.Subscribe(triggerAction))
            {
                Debug.Log($"Completed Registry event for action {KeyName}.");
            }
        }

        public void UnsubscribeRequirement()
        {
            if (string.IsNullOrEmpty(KeyName)) return;

            if (ActionIdentifyStorage.Instance.TryToGetLoadEvent(KeyName, out var triggerAction))
            {
                this.UnSubscribe(triggerAction);

                Debug.Log($"Completed UnRegistry event for action {KeyName}.");
            }
        }
    }
}
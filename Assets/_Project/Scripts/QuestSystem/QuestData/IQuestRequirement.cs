using System;
using Systems.GameAction;
using Systems.GeneralSystem.RequirementSystem;
using TnieYuPackage.ObjectDataSystem;
using UnityEngine;

namespace Systems.QuestSystem.QuestData
{
    public interface IQuestRequirement
    {
        Quest Quest { get; set; }
        bool CheckValue { get; set; }

        public void OnQuestRequirementCompleted()
        {
            Quest.LoadQuestRequirements();
        }

        void SubscribeRequirement();
        void UnsubscribeRequirement();
    }

    [Serializable]
    public class QuestFieldRequirement : FieldRequirement, IQuestRequirement
    {
        [SerializeField] private Quest quest;

        public Quest Quest
        {
            get => quest;
            set => quest = value;
        }

        public bool CheckValue
        {
            get => CheckedValue;
            set => CheckedValue = value;
        }

        public void SubscribeRequirement()
        {
            if (CheckedValue || string.IsNullOrEmpty(keyName)) return;

            if (FieldRequirementManager.Instance.TryToGetField(keyName, out var requirementValue))
            {
                requirementValue.OnValueLoad.TryToAddTrigger(OnFieldCatchingTrigger);

                Debug.Log($"Completed Subscribe field Field Requirement {keyName},");
            }
        }

        public void UnsubscribeRequirement()
        {
            if (string.IsNullOrEmpty(keyName)) return;

            if (FieldRequirementManager.Instance.TryToGetField(keyName, out var requirementValue))
            {
                requirementValue.OnValueLoad.RemoveTrigger(OnFieldCatchingTrigger);
                Debug.Log($"Completed UnRegistry event for field {keyName}.");
            }
        }

        public bool CheckValueDirectly()
        {
            if (FieldRequirementManager.Instance.TryToGetField(keyName, out var requirementValue))
            {
                return Validate(requirementValue.Value);
            }

            return false;
        }

        private bool OnFieldCatchingTrigger(object data)
        {
            bool result = Validate(data);

            if (result)
            {
                OnComplete();
            }
            
            return result;
        }

        public override bool Validate(object data)
        {
            if (data is IObjectData objectData && this.ValidatedData is IObjectData validatedObjectData)
            {
                return objectData.GreaterThanOrEqual(validatedObjectData);
            }

            Debug.LogError("current data is not IObjectData type.");
            return false;
        }

        public override void OnComplete()
        {
            CheckedValue = true;
            ((IQuestRequirement)this).OnQuestRequirementCompleted();
        }
    }

    [Serializable]
    public class QuestActionRequirement : ActionRequirement, IQuestRequirement
    {
        [SerializeField] private Quest quest;

        public Quest Quest
        {
            get => quest;
            set => quest = value;
        }

        public bool CheckValue
        {
            get => CheckedValue;
            set => CheckedValue = value;
        }

        public void SubscribeRequirement()
        {
            if (CheckedValue) return;

            if (ValidatedData is IGameActionCatcher gameActionCatcher)
            {
                gameActionCatcher.OnCompleted += OnComplete;
                GameActionCatcherManager.Instance.Registry(gameActionCatcher);
                Debug.Log("Completed Subscribe Action Requirement.");
            }
        }

        public void UnsubscribeRequirement()
        {
            if (GameActionCatcherManager.Instance == null) return;

            if (ValidatedData is IGameActionCatcher gameActionCatcher)
            {
                GameActionCatcherManager.Instance.UnRegistry(gameActionCatcher);
                gameActionCatcher.OnCompleted -= OnComplete;
                Debug.Log("Completed UnSubscribe Action Requirement.");
            }
        }

        public override void OnComplete()
        {
            CheckedValue = true;
            ((IQuestRequirement)this).OnQuestRequirementCompleted();
        }

        public override bool Validate(object data)
        {
            if (data is GameActionCommand gameActionCommand)
            {
                return ((IGameActionCatcher)ValidatedData).Format(gameActionCommand)
                       && ((IGameActionCatcher)ValidatedData).Catch(gameActionCommand);
            }

            return false;
        }
    }
}
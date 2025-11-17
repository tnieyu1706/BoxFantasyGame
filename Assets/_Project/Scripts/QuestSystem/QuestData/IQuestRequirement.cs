using System;
using Systems.GameAction;
using Systems.GeneralSystem.ObjectDataSystem;
using Systems.GeneralSystem.RequirementSystem;
using UnityEngine;

namespace Systems.QuestSystem.QuestData
{
    public interface IQuestRequirement
    {
        Quest Quest { get; set; }
        bool CheckValue { get; set; }

        public void CompleteResultQuest()
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
            if (CheckedValue || string.IsNullOrEmpty(KeyName)) return;

            if (FieldRequirementManager.Instance.TryToGetField(KeyName, out var requirementValue) &&
                this.Subscribe(requirementValue.OnValueLoad))
            {
                Debug.Log($"Completed Registry event for field {KeyName}.");
            }
        }

        public void UnsubscribeRequirement()
        {
            if (string.IsNullOrEmpty(KeyName)) return;
            
            if (FieldRequirementManager.Instance.TryToGetField(KeyName, out var requirementValue))
            {
                this.UnSubscribe(requirementValue.OnValueLoad);
                Debug.Log($"Completed UnRegistry event for field {KeyName}.");
            }
        }

        public bool CheckValueDirectly()
        {
            if (FieldRequirementManager.Instance.TryToGetField(KeyName, out var requirementValue))
            {
                return this.Validate(requirementValue.Value);
            }

            return false;
        }

        public override void CompleteResult()
        {
            ((IQuestRequirement)this).CompleteResultQuest();
        }

        public override bool Validate(ObjectData payload)
        {
            if (ObjectData.ValidateData(payload, GetValidatedPayload()) &&
                payload.CompareTo(GetValidatedPayload()) >= 0)
            {
                return true;
            }

            return false;
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

        public override void CompleteResult()
        {
            ((IQuestRequirement)this).CompleteResultQuest();
        }

        public override bool Validate(GameActionCommandStaticDto payload)
        {
            return GameActionCommandStaticDto.Matching(payload, GetValidatedPayload());
        }
    }
}
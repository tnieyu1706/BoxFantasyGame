using System;
using System.Collections.Generic;
using EditorAttributes;
using Systems.GameAction;
using Systems.GeneralSystem.BackgroundEffectSystem;
using Systems.GeneralSystem.RequirementSystem;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.SubAsset;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace Systems.QuestSystem.QuestData
{
    [Serializable]
    public class QuestStep : BaseSubAsset, IBackgroundEffectBehaviour
    {
        public const string QUEST_STEP_END_KEY = "Step_end";
        
        #region Base Properties

        [SerializeField]
        [FoldoutGroup(
            "Base Properties",
            nameof(stepId),
            nameof(describe),
            nameof(hint)
            )]
        private Void basePropertiesFoldout;
        
        [ReadOnly, HideProperty] public string stepId = string.Empty;
        
        [SerializeField, HideInInspector]
        private Quest quest;

        public override BaseParentAsset Parent
        {
            get => quest;
            set => quest = (Quest)value;
        }

        public string shortName;
        [TextArea(3, 10), HideProperty] public string describe;
        [TextArea(3, 10), HideProperty] public string hint;
        
        #endregion

        public List<StepActionCatching> actionCatchings = new();

        [SerializeReference, AbstractSupport(typeof(IBackgroundEffect))]
        private List<IBackgroundEffect> openEffects = new();

        [SerializeReference, AbstractSupport(typeof(IBackgroundEffect))]
        private List<IBackgroundEffect> closedEffects = new();

        public List<IBackgroundEffect> OpenEffects => openEffects;
        public List<IBackgroundEffect> CloseEffects => closedEffects;

        public void LoadStep()
        {
            this.HandleOpenBackground();

            //occur if actionCatching no catching any thing. => call end.
            if (actionCatchings == null || actionCatchings.Count == 0)
            {
                this.HandleCloseBackground();
                return;
            }

            if (!UpdateStepWithActionCatchings())
            {
                //action catching registry
                foreach (var actionCatching in actionCatchings)
                {
                    actionCatching.SubscribeRequirement();
                }
            }
        }

        public void UnLoadStep()
        {
            foreach (var actionCatching in actionCatchings)
            {
                actionCatching.UnsubscribeRequirement();
            }
        }

        public bool UpdateStepWithActionCatchings()
        {
            foreach (var actionCatching in actionCatchings)
            {
                if (!actionCatching.CheckedValue)
                    return false;
            }
            
            Debug.Log($"End step: {stepId}");
            this.HandleCloseBackground();
            return true;
        }

        [Button("Setup Action Catchings")]
        private void SetupActionCatchings()
        {
            foreach (var actionCatching in actionCatchings)
            {
                actionCatching.step = this;
            }
        }
    }

    [Serializable]
    public class StepActionCatching : ActionRequirement
    {
        public QuestStep step;

        public override void CompleteResult()
        {
            if (step == null) return;
            
            Debug.Log("Complete a action catching step");
            CheckedValue = true;
            step.UpdateStepWithActionCatchings();
        }

        public override bool Validate(GameActionCommandStaticDto payload)
        {
            return GameActionCommandStaticDto.Matching(payload, GetValidatedPayload());
        }
    }
}
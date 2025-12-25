using System;
using System.Collections.Generic;
using EditorAttributes;
using Systems.GameAction;
using Systems.GeneralSystem.BackgroundEffectSystem;
using Systems.GeneralSystem.RequirementSystem;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.SubAsset;
using UnityEngine;
using UnityEngine.Serialization;
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

        [SerializeField, HideInInspector] private Quest quest;

        public override BaseParentAsset Parent
        {
            get => quest;
            set => quest = (Quest)value;
        }

        public string shortName;
        [TextArea(3, 10), HideProperty] public string describe;
        [TextArea(3, 10), HideProperty] public string hint;

        #endregion

        public List<StepActionRequirement> actionRequirements = new();

        [SerializeReference, AbstractSupport(typeof(IBackgroundEffect))]
        private List<IBackgroundEffect> openEffects = new();

        [SerializeReference, AbstractSupport(typeof(IBackgroundEffect))]
        private List<IBackgroundEffect> closedEffects = new();

        public List<IBackgroundEffect> OpenEffects => openEffects;
        public List<IBackgroundEffect> CloseEffects => closedEffects;

        public void HandleOpenBackground()
        {
            if (OpenEffects is { Count: > 0 })
                OpenEffects.ForEach(e => e.Perform());

            foreach (var actionRe in actionRequirements)
            {
                if (!actionRe.CheckedValue)
                {
                    actionRe.Subscribe();
                }
            }
        }

        public void HandleCloseBackground()
        {
            if (CloseEffects == null || CloseEffects.Count == 0) return;

            UnLoadStep();
            CloseEffects.ForEach(e => e.Perform());
        }

        public void LoadStep()
        {
            HandleOpenBackground();

            //closing step: occur if actionCatching no catching any thing.
            if (CanCloseStep())
            {
                HandleCloseBackground();
            }
        }

        public void UnLoadStep()
        {
            foreach (var actionRe in actionRequirements)
            {
                actionRe.UnSubscribe();
            }
        }

        public bool CanCloseStep()
        {
            if (actionRequirements is null || actionRequirements.Count == 0) return true;
            
            foreach (var actionRe in actionRequirements)
            {
                if (!actionRe.CheckedValue) return false;
            }
            return true;
        }

        [Button("Setup Action Catchings")]
        private void SetupActionCatchings()
        {
            foreach (var actionCatching in actionRequirements)
            {
                actionCatching.step = this;
            }
        }
    }

    [Serializable]
    public class StepActionRequirement : ActionRequirement
    {
        public QuestStep step;

        public override void OnComplete()
        {
            if (step == null) return;

            CheckedValue = true;
            
            if (step.CanCloseStep())
            {
                step.HandleCloseBackground();
            }
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

        public void Subscribe()
        {
            if (ValidatedData is IGameActionCatcher gameActionCatcher)
            {
                gameActionCatcher.OnCompleted += OnComplete;
                GameActionCatcherManager.Instance.Registry(gameActionCatcher);
            }
        }

        public void UnSubscribe()
        {
            if (ValidatedData is IGameActionCatcher gameActionCatcher)
            {
                if (GameActionCatcherManager.Instance is null) return;

                GameActionCatcherManager.Instance.UnRegistry(gameActionCatcher);
                gameActionCatcher.OnCompleted -= OnComplete;
            }
        }
    }
}
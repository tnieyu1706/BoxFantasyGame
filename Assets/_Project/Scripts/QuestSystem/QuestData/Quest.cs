using System;
using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using Systems.GeneralSystem.BackgroundEffectSystem;
using Systems.GeneralSystem.RequirementSystem;
using Systems.QuestSystem.QuestData.Editor;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.SubAsset;
using TnieYuPackage.ObjectDataSystem;
using TnieYuPackage.Utils;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using UnityEngine.Serialization;
using Void = EditorAttributes.Void;

namespace Systems.QuestSystem.QuestData
{
    [Serializable]
    public enum QuestType
    {
        Main,
        Side
    }

    [CreateAssetMenu(fileName = "Quest", menuName = "QuestSystem/Quest")]
    public class Quest : BaseParentAsset<QuestStep>, IBackgroundEffectBehaviour
    {
        #region Tabs

        [SerializeField]
        [TabGroup(
            nameof(baseTab),
            nameof(stepTab),
            nameof(reqTab),
            nameof(effectTab)
        )]
        private Void propertiesTab;

        #region Base Properties

        [SerializeField]
        [HideProperty]
        [VerticalGroup(
            nameof(id),
            nameof(title),
            nameof(description),
            nameof(state),
            nameof(questType),
            nameof(awards)
        )]
        private Void baseTab;

        [ReadOnly, HideProperty] public SerializableGuid id = SerializableGuid.NewGuid();

        [HideProperty] public string title;
        [TextArea(3, 10), HideProperty] public string description;

        [SerializeReference, QuestStateDrawer, HideProperty] public BaseQuestState state;
        [HideProperty] public QuestType questType = QuestType.Side;

        [HideProperty] public SerializableDictionaryAbstract<string, ObjectData> awards;

        #endregion

        #region Step Properties

        [SerializeField]
        [HideProperty]
        [VerticalGroup(
            nameof(currentProcessing),
            nameof(steps)
        )]
        private Void stepTab;

        [HideProperty] public List<QuestStep> steps;

        #region RuntimeFields

        [HideInInspector] public QuestStep currentStep;
        [HideProperty] public string currentProcessing;

        #endregion

        #endregion

        #region Requirement Properties

        [SerializeField]
        [HideProperty]
        [VerticalGroup(
            nameof(requirements)
        )]
        private Void reqTab;

        [SerializeReference, AbstractSupport(typeof(IRequirement)), HideProperty]
        public List<IQuestRequirement> requirements;

        #endregion

        #region Background Effects Properties

        [SerializeField]
        [HideProperty]
        [VerticalGroup(
            nameof(openEffects),
            nameof(closedEffects)
        )]
        private Void effectTab;

        [SerializeReference, AbstractSupport(typeof(IBackgroundEffect)), HideProperty]
        private List<IBackgroundEffect> openEffects = new();

        [SerializeReference, AbstractSupport(typeof(IBackgroundEffect)), HideProperty]
        private List<IBackgroundEffect> closedEffects = new();

        public List<IBackgroundEffect> OpenEffects => openEffects;
        public List<IBackgroundEffect> CloseEffects => closedEffects;

        #endregion

        #endregion

        #region SubAsset Handler

        public override List<QuestStep> SubAssets => steps;

        protected override void HandleSubAssetWhenGenerating(QuestStep subAsset)
        {
        }

        protected override void HandleSubAssetBeforeDelete(QuestStep subAsset)
        {
        }

        #endregion

        #region Button Supports

        [Button]
        private void SetupRequirements()
        {
            foreach (var re in requirements)
            {
                if (re == null) continue;

                re.Quest = this;
            }
        }

        [Button]
        private void SetupSteps()
        {
            int i = 0;
            foreach (var step in steps)
            {
                step.stepId = "step_" + (++i);
            }
        }

        [Button]
        private void ResetQuest()
        {
            state = new LockState(this);
            if (requirements is { Count: > 0 })
            {
                foreach (var re in requirements)
                {
                    re.CheckValue = false;
                }
            }

            if (steps is { Count: > 0 })
            {
                foreach (var step in steps)
                {
                    if (step.actionCatchings is { Count: > 0 })
                    {
                        foreach (var action in step.actionCatchings)
                        {
                            action.CheckedValue = false;
                        }
                    }
                }

                currentProcessing = string.Empty;
            }
        }

        [Button]
        private void CloseQuestManual()
        {
            foreach (var re in requirements)
            {
                re.CheckValue = true;
            }
            
            foreach (var step in steps)
            {
                step.actionCatchings.ForEach(a => a.CheckedValue = true);
            }
            
            BaseQuestState.Transition<ClosedState>(ref state, this);
        }

        #endregion

        #region Quest Methods

        public bool LoadQuestRequirements()
        {
            if (requirements is { Count: > 0 })
            {
                //load FieldRequirement when not catching
                foreach (var reField in requirements.OfType<QuestFieldRequirement>())
                {
                    if (!reField.CheckValue)
                    {
                        reField.CheckedValue = reField.CheckValueDirectly();
                    }
                }

                foreach (var re in requirements)
                {
                    if (re == null) continue;

                    if (!re.CheckValue)
                        return false;
                }
            }

            OpenQuest();
            return true;
        }

        public Dictionary<string, QuestStep> GetMapSteps()
        {
            return steps.ToDictionary(s => s.stepId);
        }

        public void LoadCurrentStep()
        {
            if (string.IsNullOrEmpty(currentProcessing))
            {
                if (currentProcessing.Equals(QuestStep.QUEST_STEP_END_KEY))
                {
                    Debug.Log($"Current quest - {id} is end");
                    if (state is not ClosedState)
                    {
                        CloseQuest();
                    }

                    return;
                }
                
                if (steps == null || steps.Count == 0)
                {
                    Debug.Log($"Current quest - {id} don't have any steps.");
                    CloseQuest();
                    return;
                }

                currentProcessing = steps[0].stepId;
            }

            if (GetMapSteps().TryGetValue(currentProcessing, out currentStep))
            {
                currentStep.LoadStep();
                Debug.Log($"Successfully loaded the current quest step {id} - {currentStep.stepId}");
                return;
            }

            Debug.Log("Failed to load the current quest step");
        }

        private void PerformAwards()
        {
            if (awards == null || awards.data.Count <= 0) return;

            FieldRequirementValue fieldValue;
            foreach (var kvp in awards.Dictionary)
            {
                if (FieldRequirementManager.Instance.TryToGetField(kvp.Key, out fieldValue))
                {
                    fieldValue.Value += kvp.Value;
                }
            }
        }

        public void OpenQuest()
        {
            BaseQuestState.Transition<UnlockedState>(ref state, this);
            state.Do();
            
            this.HandleOpenBackground();

            if (steps is { Count: > 0 })
            {
                currentProcessing = steps[0].stepId;
                LoadCurrentStep();
            }
            else
            {
                CloseQuest();
            }

            Debug.Log("The quest is unlocked");
        }

        public void CloseQuest()
        {
            BaseQuestState.Transition<ClosedState>(ref state, this);
            state.Do();

            //Do something when quest closed.
            PerformAwards();
            this.HandleCloseBackground();
            currentProcessing = QuestStep.QUEST_STEP_END_KEY;
            currentStep = null;
            Debug.Log("The quest is closed");
        }

        public bool MoveNextStep(string stepId)
        {
            if (string.IsNullOrEmpty(stepId) || !GetMapSteps().ContainsKey(stepId))
            {
                return false;
            }

            //ensure unRegistry all old step
            if (currentStep != null)
            {
                currentStep.UnLoadStep(); //ensure
            }

            //normal execute move next step.
            currentProcessing = stepId;
            LoadCurrentStep();
            return true;
        }

        #endregion

        #region Load/UnLoad LifeCycle

        public void RegistryRequirement()
        {
            if (!LoadQuestRequirements())
            {
                foreach (var re in requirements)
                {
                    if (re != null) re.SubscribeRequirement();
                }
            }
        }

        public void UnRegistryRequirement()
        {
            foreach (var re in requirements)
            {
                re?.UnsubscribeRequirement();
            }
        }

        public void StartCycle()
        {
            state.Do();
        }

        public void EndCycle()
        {
            if (state is CheckingState && requirements is { Count: > 0 })
            {
                UnRegistryRequirement();
            }
            else if (state is UnlockedState && currentStep != null)
            {
                currentStep.UnLoadStep();
            }
        }

        #endregion
    }
}
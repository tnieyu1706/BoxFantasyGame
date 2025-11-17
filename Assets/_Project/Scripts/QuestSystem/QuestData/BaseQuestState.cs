using System;
using Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime;
using UnityEngine;

namespace Systems.QuestSystem.QuestData
{
    [Serializable]
    public abstract class BaseQuestState
    {
        public Quest quest;

        protected BaseQuestState()
        {
        }

        protected BaseQuestState(Quest quest)
        {
            this.quest = quest;
        }

        public abstract void Entry();
        public abstract void Do();
        public abstract void Exit();

        public static void Transition<TState>(ref BaseQuestState state, Quest quest)
            where TState : BaseQuestState
        {
            if (typeof(BaseQuestState) == typeof(TState))
            {
                Debug.LogWarning("can't handle BaseQuestState transition");
                return;
            }

            state?.Exit();

            state = (TState)Activator.CreateInstance(typeof(TState));
            state.quest = quest;

            state.Entry();
        }
    }

    [Serializable]
    public class LockState : BaseQuestState
    {
        public LockState() : base()
        {
        }

        public LockState(Quest quest) : base(quest)
        {
        }

        public override void Entry()
        {
        }

        public override void Do()
        {
        }

        public override void Exit()
        {
        }

        public override string ToString() => "Lock";
    }

    [Serializable]
    public class CheckingState : BaseQuestState
    {
        public CheckingState() : base()
        {
        }

        public CheckingState(Quest quest) : base(quest)
        {
        }

        public override void Entry()
        {
            //registry to checkingList
            if (QuestFlowGraphDirector.Instance != null &&
                !QuestFlowGraphDirector.Instance.checkingQuests.Contains(quest))
            {
                QuestFlowGraphDirector.Instance.checkingQuests.Add(quest);
            }
            
            if (quest.requirements == null || quest.requirements.Count == 0)
            {
                quest.OpenQuest(); //ensure
            }
        }

        public override void Do()
        {
            //runtime executed
            quest.RegistryRequirement();
        }

        public override void Exit()
        {
            //remove from checkingList
            if (QuestFlowGraphDirector.Instance != null &&
                QuestFlowGraphDirector.Instance.checkingQuests.Contains(quest))
            {
                QuestFlowGraphDirector.Instance.checkingQuests.Remove(quest);
            }
            
            quest.UnRegistryRequirement();
        }

        public override string ToString() => "Checking";
    }

    [Serializable]
    public class UnlockedState : BaseQuestState
    {
        public UnlockedState() : base()
        {
        }

        public UnlockedState(Quest quest) : base(quest)
        {
        }

        public override void Entry()
        {
            //registry to unlockedList
            if (QuestFlowGraphDirector.Instance != null &&
                !QuestFlowGraphDirector.Instance.unlockedQuests.Contains(quest))
            {
                QuestFlowGraphDirector.Instance.unlockedQuests.Add(quest);
            }
        }

        public override void Do()
        {
            quest.LoadCurrentStep();
            quest.currentStep.LoadStep(); //ensure 
        }

        public override void Exit()
        {
            //remove from unlockedList
            if (QuestFlowGraphDirector.Instance != null &&
                QuestFlowGraphDirector.Instance.unlockedQuests.Contains(quest))
            {
                QuestFlowGraphDirector.Instance.unlockedQuests.Remove(quest);
            }
        }

        public override string ToString() => "Unlocked";
    }

    [Serializable]
    public class ClosedState : BaseQuestState
    {
        public ClosedState() : base()
        {
        }

        public ClosedState(Quest quest) : base(quest)
        {
        }

        public override void Entry()
        {
        }

        public override void Do()
        {
        }

        public override void Exit()
        {
        }

        public override string ToString() => "Closed";
    }
}
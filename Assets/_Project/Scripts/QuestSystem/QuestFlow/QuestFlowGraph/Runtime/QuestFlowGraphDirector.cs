using System;
using System.Collections.Generic;
using EditorAttributes;
using Systems.QuestSystem.QuestData;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime
{
    public class QuestFlowGraphDirector : SingletonBehavior<QuestFlowGraphDirector>
    {
        public QuestFlowGraphRuntime questFlowGraphRuntime;

        [ReadOnly] public List<Quest> unlockedQuests = new();
        [ReadOnly] public List<Quest> checkingQuests = new();

        void OnEnable()
        {
            UpdateAllFlow();
        }

        private void OnDisable()
        {
            UnloadAllQuests();
        }

        [Button]
        public void UpdateAllFlow()
        {
            if (questFlowGraphRuntime != null)
            {
                questFlowGraphRuntime.UpdateAllQuestFlow();
            }

            UpdateUnlockedQuests();
            UpdateCheckingQuests();
        }

        private void UpdateUnlockedQuests()
        {
            if (unlockedQuests != null)
            {
                //unRegistry old
                foreach (var quest in unlockedQuests)
                {
                    quest.EndCycle();
                }
            }

            unlockedQuests = questFlowGraphRuntime.GetUnlockedQuests();
            foreach (var quest in unlockedQuests)
            {
                quest.StartCycle();
            }
        }

        private void UpdateCheckingQuests()
        {
            if (checkingQuests != null)
            {
                //unRegistry old
                foreach (var quest in checkingQuests)
                {
                    quest.EndCycle();
                }
            }

            checkingQuests = questFlowGraphRuntime.GetCheckingQuests();

            foreach (var quest in checkingQuests)
            {
                quest.StartCycle();
            }
        }

        private void UnloadAllQuests()
        {
            List<Quest> unloadedQuests = questFlowGraphRuntime.GetUnlockedQuests();
            unloadedQuests.AddRange(questFlowGraphRuntime.GetCheckingQuests());
            
            foreach (var q in unloadedQuests)
            {
                q.EndCycle();
            }
        }
    }
}
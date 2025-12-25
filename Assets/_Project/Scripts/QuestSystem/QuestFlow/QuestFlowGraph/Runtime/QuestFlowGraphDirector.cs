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
        private void UpdateAllFlow()
        {
            if (questFlowGraphRuntime == null) return;

            questFlowGraphRuntime.UpdateAllQuestFlow();

            LoadUnlockedQuests();
            LoadCheckingQuests();
        }

        private void LoadUnlockedQuests()
        {
            //unRegistry old
            foreach (var quest in unlockedQuests)
            {
                quest.EndCycle();
            }

            unlockedQuests = questFlowGraphRuntime.GetUnlockedQuests();
            if (unlockedQuests == null) return;
            
            foreach (var quest in unlockedQuests)
            {
                quest.StartCycle();
            }
        }

        private void LoadCheckingQuests()
        {
            //unRegistry old
            foreach (var quest in checkingQuests)
            {
                quest.EndCycle();
            }

            checkingQuests = questFlowGraphRuntime.GetCheckingQuests();
            if (checkingQuests == null) return;

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
using System.Collections.Generic;
using System.Linq;
using Systems.QuestSystem.QuestData;
using Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime.Nodes;
using UnityEngine;

namespace Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime
{
    public class QuestFlowGraphRuntime : ScriptableObject
    {
        public List<QuestNodeRuntime> nodes = new List<QuestNodeRuntime>();

        public Dictionary<Quest, QuestNodeRuntime> GetMap()
        {
            return nodes.ToDictionary(n => n.quest);
        }

        public List<Quest> GetCheckingQuests()
        {
            return nodes.Select(node => node.quest)
                .Where(q => q.state is CheckingState)
                .ToList();
        }

        public List<Quest> GetUnlockedQuests()
        {
            return nodes.Select(node => node.quest)
                .Where(q => q.state is UnlockedState)
                .ToList();
        }

        /// <summary>
        /// UpdateCloseQuest to nextQuests and update state but not yet call state.Do()
        /// </summary>
        /// <param name="updatedQuest"></param>
        /// <returns></returns>
        public List<Quest> UpdateClosedQuestForQuestFlow(Quest updatedQuest)
        {
            if (updatedQuest == null || updatedQuest.state is not ClosedState)
                return null;

            if (GetMap().TryGetValue(updatedQuest, out QuestNodeRuntime questNode))
            {
                List<Quest> updatedQuests = new();
                QuestNodeRuntime nextQuestNode;
                foreach (var nextQuest in questNode.nextQuests)
                {
                    if (GetMap().TryGetValue(nextQuest, out nextQuestNode))
                    {
                        if (QuestFlowSupport.ValidateEligibilityChecking(nextQuestNode))
                        {
                            BaseQuestState.Transition<CheckingState>(ref nextQuest.state, nextQuest);
                            
                            //record
                            updatedQuests.Add(nextQuest);
                        }
                    }
                    else 
                        Debug.LogWarning($"{nextQuest} don't exist in QuestFlowGraph");
                }
                
                return updatedQuests;
            }
            else
            {
                Debug.LogWarning("Quest " + updatedQuest.name + " has no quest node running.");
                return null;
            }
        }

        /// <summary>
        /// Update all questFlow & ensure all quest is valid context state.
        /// </summary>
        public void UpdateAllQuestFlow()
        {
            foreach (var questNode in nodes)
            {
                
                if (questNode.quest.state is LockState)
                {
                    if (QuestFlowSupport.ValidateEligibilityChecking(questNode))
                    {
                        BaseQuestState.Transition<CheckingState>(ref questNode.quest.state, questNode.quest);
                    }
                    
                    if (questNode.preQuests == null || questNode.preQuests.Count == 0)
                    {
                        BaseQuestState.Transition<CheckingState>(ref questNode.quest.state, questNode.quest);
                    }
                }

                if (questNode.quest.state is CheckingState)
                {
                    questNode.quest.LoadQuestRequirements();
                }
            }
            Debug.Log("Quest update all quest flow completed.");
        }
    }

    public static class QuestFlowSupport
    {
        public static bool ValidateEligibilityChecking(QuestNodeRuntime questNode)
        {
            foreach (var preQuest in questNode.preQuests)
            {
                if (preQuest.state is not ClosedState)
                    return false;
            }

            return true;
        }
    }
}
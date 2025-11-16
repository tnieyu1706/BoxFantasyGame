using System;
using Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime;
using UnityEngine;

namespace Systems.QuestSystem.BackgroundEffect
{
    [Serializable]
    public class CloseQuestBackgroundEffect : BaseQuestBackgroundEffect
    {
        public QuestFlowGraphRuntime questFlow;
        public override void Perform()
        {
            if (quest == null) return;
            
            quest.CloseQuest();

            if (questFlow == null)
            {
                Debug.LogWarning("No quest flow running.");
                return;
            }
            
            questFlow.UpdateClosedQuestForQuestFlow(quest);
        }
    }
}
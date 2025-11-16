using System;
using UnityEngine;

namespace Systems.QuestSystem.BackgroundEffect
{
    [Serializable]
    public class MoveNextQuestBackgroundEffect : BaseQuestBackgroundEffect
    {
        public string nextStepId;
        public override void Perform()
        {
            if (quest == null) return;

            if (quest.MoveNextStep(nextStepId))
            {
                Debug.Log("Moved Next Step: " + nextStepId);
                return;
            }
            
            Debug.LogWarning($"{nextStepId} not found in quest steps.");
        }
    }
}
using System;
using Systems.GeneralSystem.BackgroundEffectSystem;
using Systems.QuestSystem.QuestData;

namespace Systems.QuestSystem.BackgroundEffect
{
    [Serializable]
    public abstract class BaseQuestBackgroundEffect : IBackgroundEffect
    {
        public Quest quest;
        public abstract void Perform();
    }
}
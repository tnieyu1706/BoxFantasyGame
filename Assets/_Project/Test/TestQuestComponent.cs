using Systems.QuestSystem.QuestData;
using UnityEngine;

namespace _Project.Test
{
    public class TestQuestComponent : MonoBehaviour
    {
        public Quest quest;


        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                OpenQuest();
            }
        }

        void OpenQuest()
        {
            if (quest == null) return;

            if (quest.state is not UnlockedState) return;
            
            quest.LoadCurrentStep();
        }
    }
}
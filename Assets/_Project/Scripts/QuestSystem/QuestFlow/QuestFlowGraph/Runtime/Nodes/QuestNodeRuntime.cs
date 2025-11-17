using System;
using System.Collections.Generic;
using Systems.QuestSystem.QuestData;
using TnieYuPackage.Utils;

namespace Systems.QuestSystem.QuestFlow.QuestFlowGraph.Runtime.Nodes
{
    [Serializable]
    public class QuestNodeRuntime
    {
        public SerializableGuid id = SerializableGuid.Empty;
        public Quest quest;
        public List<Quest> preQuests;
        public List<Quest> nextQuests;
    }
}
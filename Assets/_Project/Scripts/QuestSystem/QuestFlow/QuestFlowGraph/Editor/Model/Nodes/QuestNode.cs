using System;
using Systems.QuestSystem.QuestData;
using TnieYuPackage.GTKExtensions;
using Unity.GraphToolkit.Editor;

namespace Systems.QuestSystem.QuestFlow.QuestFlowGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class QuestNode : Node
    {
        public const string NUMBER_PREQUEST_NAME = "PreQuestNumber";
        public const string PREQUEST_NAME = "PreQuest";
        public const string NEXTQUEST_NAME = "NextQuest";
        public const string QUEST_OPTION_NAME = "Quest";
        private const string STATE_PORT_NAME = "State";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<Quest>(QUEST_OPTION_NAME)
                .Delayed()
                .Build();
            
            context.AddOption<int>(NUMBER_PREQUEST_NAME)
                .Delayed()
                .Build();
            
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            Quest currentQuest = this.GetNodeOptionValue<Quest>(QUEST_OPTION_NAME);
            if (currentQuest != null && currentQuest.state != null)
            {
                context.AddOutputPort("State")
                    .WithDisplayName($"State: {currentQuest.state}")
                    .Build();
            }
            
            var preQuestNumber = this.GetNodeOptionValue<int>(NUMBER_PREQUEST_NAME);
            if (preQuestNumber > 0)
            {
                for (int i = 0; i < preQuestNumber; i++)
                {
                    context.AddInputPort(PREQUEST_NAME+i)
                        .WithDisplayName(string.Empty)
                        .WithConnectorUI(PortConnectorUI.Arrowhead)
                        .Build();
                }
            }

            context.AddOutputPort(NEXTQUEST_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
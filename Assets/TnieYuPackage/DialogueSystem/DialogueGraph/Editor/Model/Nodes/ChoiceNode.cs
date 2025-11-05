using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class ChoiceNode : BaseDialogueNode
    {
        public const string CHOICE_NUMBER_NAME = "ChoiceNumber";
        public const string CHOICE_TEXT_DEFAULT_NAME = "ChoiceText_";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(CHOICE_NUMBER_NAME)
                .WithDisplayName(CHOICE_NUMBER_NAME)
                .WithDefaultValue(2)
                .Delayed()
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            var choiceNumberOption = GetNodeOptionByName(CHOICE_NUMBER_NAME);
            if (choiceNumberOption == null)
            {
                Debug.LogWarning("Choice number not defined");
                return;
            }

            if (choiceNumberOption.TryGetValue(out int choiceNumber))
            {
                for (int i = 0; i < choiceNumber; i++)
                {
                    context.AddInputPort<string>(CHOICE_TEXT_DEFAULT_NAME + i)
                        .Build();

                    context.AddOutputPort("Choice" + EXECUTION_PORT_DEFAULT_NAME + i)
                        .WithDisplayName("Choice" + i)
                        .WithConnectorUI(PortConnectorUI.Arrowhead)
                        .Build();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using TnieYuPackage.DesignPatterns.Patterns.Builder;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueSheet.Models
{
    [Serializable]
    public record DialogueNodeDto : IMyBuilder<DialogueNode>
    {
        [SerializeField] private string id;
        [SerializeField] private string characterName;
        [SerializeField] private string content;
        [SerializeField] private string choice1;
        [SerializeField] private string choice2;
        [SerializeField] private string choice3;
        [SerializeField] private string next;
        [SerializeField] private string actionName;

        public string Id
        {
            get => id;
            set => id = value;
        }

        public string CharacterName
        {
            get => characterName;
            set => characterName = value;
        }

        public string Content
        {
            get => content;
            set => content = value;
        }

        public string Choice1
        {
            get => choice1;
            set => choice1 = value;
        }

        public string Choice2
        {
            get => choice2;
            set => choice2 = value;
        }

        public string Choice3
        {
            get => choice3;
            set => choice3 = value;
        }

        public string Next
        {
            get => next;
            set => next = value;
        }

        public string ActionName
        {
            get => actionName;
            set => actionName = value;
        }

        private bool ChoiceTextToChoice(string choiceText, out Choice choice)
        {
            if (string.IsNullOrEmpty(choiceText) || !choiceText.Contains('#'))
            {
                Debug.LogWarning($"Current choice {choiceText} is not a valid choice.");
                choice = default;
                return false;
            }

            var splits = choiceText.Split('#');
            choice = new Choice()
            {
                text = splits[0],
                next = splits[1],
            };
            return true;
        }

        public DialogueNode Build()
        {
            var dialogueNode = new DialogueNode
            {
                id = Id,
                characterName = characterName,
                content = Content,
                next = Next,
                actionName = ActionName,
            };
            
            if (choice1 == null && choice2 == null && choice3 == null) 
                return dialogueNode;

            List<Choice> choices = new();

            foreach (var choiceText in new[] { choice1, choice2, choice3 })
            {
                if (!string.IsNullOrEmpty(choiceText) && ChoiceTextToChoice(choiceText, out Choice choice))
                {
                    choices.Add(choice);
                }
            }
            dialogueNode.choices = choices;
            
            return dialogueNode;
        }
    }
}
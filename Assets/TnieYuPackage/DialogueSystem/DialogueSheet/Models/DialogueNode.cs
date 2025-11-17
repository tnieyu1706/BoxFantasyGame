using System;
using System.Collections.Generic;

namespace TnieYuPackage.DialogueSystem.DialogueSheet.Models
{
    [Serializable]
    public struct Choice
    {
        public string text;
        public string next;
    }

    [Serializable]
    public class DialogueNode
    {
        public string id;
        public string characterName;
        public string content;
        public List<Choice> choices;
        public string next;
        public string actionName;
        
        public bool IsChoiceNode => choices != null && choices.Count > 0;
    }
}
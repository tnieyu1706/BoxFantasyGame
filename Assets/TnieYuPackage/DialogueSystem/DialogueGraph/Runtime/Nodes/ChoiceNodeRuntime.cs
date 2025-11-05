using System;
using System.Collections.Generic;
using TnieYuPackage.Utils;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class ChoiceNodeRuntime : BaseDialogueNodeRuntime<ChoiceNodeRuntime>
    {
        public List<ChoiceOption> choices;

        public ChoiceNodeRuntime(SerializableGuid id, string character, string content, List<ChoiceOption> choices) : base(id, character, content)
        {
            this.choices = choices;
        }
    }
    
    [Serializable]
    public class ChoiceOption
    {
        public string text;
        public SerializableGuid nextId = SerializableGuid.Empty;
    }
}
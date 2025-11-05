using System;
using TnieYuPackage.Utils;
using UnityEngine.Serialization;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    public interface IBaseDialogueNodeRuntime
    {
        string Character { get; set; }
        string Content { get; set; }
    }
    
    [Serializable]
    public abstract class BaseDialogueNodeRuntime<T> : NodeRuntime<T>, IBaseDialogueNodeRuntime
        where T : BaseDialogueNodeRuntime<T>
    {
        public string character;
        public string content;

        public string Character
        {
            get => character;
            set => character = value;
        }

        public string Content
        {
            get => content;
            set => content = value;
        }


        protected BaseDialogueNodeRuntime(
            SerializableGuid id,
            string character,
            string content
        ) : base(id)
        {
            this.Character = character;
            this.Content = content;
        }
    }

    [Serializable]
    public class DialogueNodeRuntime : BaseDialogueNodeRuntime<DialogueNodeRuntime>
    {
        public SerializableGuid nextId = SerializableGuid.Empty;

        public DialogueNodeRuntime(SerializableGuid id, string character, string content) : base(id, character, content)
        {
        }
    }
}
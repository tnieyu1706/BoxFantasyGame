using System;
using System.Collections.Generic;
using System.Linq;
using TnieYuPackage.DesignPatterns.Patterns.Builder;
using TnieYuPackage.FileData;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using UnityEngine.Events;

namespace TnieYuPackage.DialogueSystem.DialogueSheet.Models
{
    [Serializable]
    public class DialogueSheet
    {
        public static DialogueSheet Instance { get; set; }
        
        public SerializableDictionary<string, DialogueNode> nodes = new();
        public SerializableDictionary<string, UnityEvent> actions = new();
        
        public class Builder : IMyBuilder<DialogueSheet>
        {
            private readonly DialogueSheet dialogueSheet = new();

            public Builder BuildDialogueNodes(IFileData fileData)
            {
                if (fileData == null || string.IsNullOrEmpty(fileData.Path))
                {
                    Debug.LogWarning("No data loaded!");
                    return this;
                }

                //load data from csv
                IEnumerable<DialogueNodeDto> nodesDto = fileData.ReadData<DialogueNodeDto>();
                dialogueSheet.nodes.data =
                    nodesDto.Select(dto => dto.Build())
                        .Select(
                            d =>
                                new SerializableKeyPair<string, DialogueNode>(d.id, d)
                        )
                        .ToList();

                return this;
            }

            public Builder BuildActions(Dictionary<string, UnityEvent> actions)
            {
                dialogueSheet.actions.AddRange(actions);
                
                return this;
            }

            public DialogueSheet Build()
            {
                return dialogueSheet;
            }
        }
    }

    
}
using System.Collections.Generic;
using System.Linq;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes;
using TnieYuPackage.ObjectDataSystem;
using TnieYuPackage.Utils;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using UnityEngine.Events;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime
{
    public class DialogueGraphRuntime : ScriptableObject
    {
        public static DialogueGraphRuntime CurrentGraphRuntime { get; set; }

        [SerializeReference] public List<NodeRuntime> Nodes = new();
        public List<FieldNodeRuntime> Fields = new();
        public SerializableDictionary<string, UnityEvent> MethodBindings = new();
        
        public SerializableDictionaryAbstract<SerializableGuid, IObjectData> datasBackup = new();
        public bool isBackup;

        public void BackupFieldData()
        {
            if (isBackup)
            {
                foreach (var field in Fields)
                {
                    field.data.Value = datasBackup[field.id].Value;
                }
            }
        }

        public Dictionary<SerializableGuid, FieldNodeRuntime> FieldsDict =>
            Fields.ToDictionary(f => f.id, f => f);

        public Dictionary<SerializableGuid, NodeRuntime> NodesDict => Nodes.ToDictionary(n => n.id, f => f);
    }
}
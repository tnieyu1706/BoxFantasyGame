using System.Collections.Generic;
using System.Linq;
using Systems.GeneralSystem.ObjectDataSystem;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes;
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
        
        [SerializeReference]
        public List<ObjectData> datasBackup = new();
        public bool isBackup;

        public void BackupFieldData()
        {
            if (isBackup)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    Fields[i].data = datasBackup[i].Clone() as ObjectData;
                }
            }
        }

        public Dictionary<SerializableGuid, FieldNodeRuntime> FieldsDict =>
            Fields.ToDictionary(f => f.id, f => f);

        public Dictionary<SerializableGuid, NodeRuntime> NodesDict => Nodes.ToDictionary(n => n.id, f => f);
    }
}
using System;
using TnieYuPackage.Utils;
using UnityEngine.Serialization;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    public interface IFlowNodeRuntime
    {
        SerializableGuid NextId { get; set; }
        void Execute();
    }
    
    /// <summary>
    /// FlowNodeRuntime is a class for node when Handle it will skip for next node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class FlowNodeRuntime<T> : NodeRuntime<T>, IFlowNodeRuntime
        where T : FlowNodeRuntime<T>
    {
        public SerializableGuid nextId = SerializableGuid.Empty;
        public SerializableGuid NextId
        {
            get => nextId;
            set => nextId = value;
        }

        protected FlowNodeRuntime(SerializableGuid id) : base(id)
        {
        }

        public abstract void Execute();
    }
}
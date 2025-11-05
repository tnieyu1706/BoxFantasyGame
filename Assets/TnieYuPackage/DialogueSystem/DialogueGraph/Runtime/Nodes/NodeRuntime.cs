using System;
using System.Reflection;
using JetBrains.Annotations;
using TnieYuPackage.Utils;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    /// <summary>
    /// NodeRuntime is basic node runtime every runtime have to implement
    /// </summary>
    [Serializable]
    public abstract class NodeRuntime
    {
        public const string HANDLE_ACTION_NAME = "Handle";
        
        public SerializableGuid id = SerializableGuid.Empty;
        protected NodeRuntime(SerializableGuid id)
        {
            this.id = id;
        }

        [CanBeNull]
        public static Action<NodeRuntime> GetActualHandleContext(NodeRuntime context)
        {
            var contextType = context.GetType();
            var nodeRuntimeGenericType = typeof(NodeRuntime<>).MakeGenericType(contextType);

            if (!nodeRuntimeGenericType.IsAssignableFrom(contextType))
            {
                Debug.LogError($"contextType {contextType} is not assignable to NodeRuntime<>");
                return null;
            }
            
            var handleMethod = nodeRuntimeGenericType.GetField(HANDLE_ACTION_NAME, BindingFlags.Static | BindingFlags.Public);

            if (handleMethod == null)
            {
                Debug.LogWarning($"{context.GetType().FullName} is not a static event Handle.");
                return null;
            }

            return handleMethod.GetValue(null) as Action<NodeRuntime>;
        }
    }
    
    /// <summary>
    /// NodeRuntime<T> basic node runtime with Handle static.
    /// Handle event will declare by export.
    /// Get by Handle context by NodeRuntime class.
    /// </summary>
    /// <typeparam name="T">T is class implement type.</typeparam>
    [Serializable]
    public abstract class NodeRuntime<T> : NodeRuntime
        where T : NodeRuntime
    {
        public static Action<NodeRuntime> Handle;

        protected NodeRuntime(SerializableGuid id) : base(id)
        {
        }
    }
}
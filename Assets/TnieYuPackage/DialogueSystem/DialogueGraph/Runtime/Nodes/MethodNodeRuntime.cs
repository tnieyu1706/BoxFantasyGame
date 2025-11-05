using System;
using TnieYuPackage.DialogueSystem.DialogueGraph.Runtime;
using TnieYuPackage.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class MethodNodeRuntime : FlowNodeRuntime<MethodNodeRuntime>
    {
        public string methodName;

        public MethodNodeRuntime(SerializableGuid id, string methodName) : base(id)
        {
            this.methodName = methodName;
        }

        public override void Execute()
        {
            var graph = DialogueGraphRuntime.CurrentGraphRuntime;
            if (graph == null)
            {
                Debug.LogWarning("Graph runtime is null. Graph runtime will not be executed.");
                return;
            }

            graph.MethodBindings.Dictionary.TryGetValue(methodName, out UnityEvent method);
            if (method == null)
            {
                Debug.Log($"Current graph runtime not registry any {methodName} method.");
                return;
            }
            
            method.Invoke();
        }
    }
}
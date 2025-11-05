using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal abstract class CheckedNode : Node
    {
        public override void OnEnable()
        {
            //check value type when save. (refresh)
            var inputPorts = this.GetInputPorts();
            var outputPorts = this.GetOutputPorts();

            var ports = inputPorts.Concat(outputPorts);

            Type currentType;
            List<IPort> connectedPorts = new();
            foreach (var port in ports)
            {
                if (port.isConnected)
                {
                    //check
                    currentType = port.dataType;
                    
                    connectedPorts.Clear();
                    port.GetConnectedPorts(connectedPorts);

                    foreach (var connectedType in connectedPorts.Select(p => p.dataType))
                    {
                        if (connectedType?.FullName != null && !connectedType.FullName.Equals(currentType.FullName))
                        {
                            Debug.LogWarning($"{connectedType.FullName} is not the same as {currentType.FullName}");
                        }
                    }
                }
            }
        }
    }
}
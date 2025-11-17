using System;
using Unity.GraphToolkit.Editor;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal abstract class BaseNode : Node
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";
    }
}
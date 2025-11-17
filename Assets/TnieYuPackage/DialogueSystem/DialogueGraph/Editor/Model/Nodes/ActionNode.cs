using System;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    /// <summary>
    /// Action Node is a action have flow from - to
    /// but objective is (Do) action follow context.
    /// </summary>
    [Serializable]
    internal abstract class ActionNode : FlowNode
    {
    }
}
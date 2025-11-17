using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Editor.Model.Nodes
{
    [Serializable]
    internal class AllField : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputPortType<int>(context, nameof(Int32));
            AddInputPortType<float>(context, nameof(Single));
            AddInputPortType<string>(context, nameof(String));
            AddInputPortType<bool>(context, nameof(Boolean));
            AddInputPortType<Vector2>(context, nameof(Vector2));
            AddInputPortType<Vector3>(context, nameof(Vector3));
            AddInputPortType<Vector4>(context, nameof(Vector4));
            AddInputPortType<Color>(context, nameof(Color));
            AddInputPortType<Quaternion>(context, nameof(Quaternion));
            AddInputPortType<GameObject>(context, nameof(GameObject));
            AddInputPortType<Texture2D>(context, nameof(Texture2D));
            AddInputPortType<Mesh>(context, nameof(Mesh));
            AddInputPortType<AnimationCurve>(context, nameof(AnimationCurve));
        }

        private void AddInputPortType<T>(IPortDefinitionContext ctx, string portName)
        {
            ctx.AddInputPort<T>(portName)
                .Build();
        }
    }
}
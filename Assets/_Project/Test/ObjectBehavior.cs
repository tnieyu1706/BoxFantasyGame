using UnityEngine;

namespace _Project.Test
{
    public interface IObjectBehavior
    {
        public string Value { get; }
    }
    public class ObjectBehavior : MonoBehaviour, IObjectBehavior
    {
        public string value = "hello xin chao";
        public string Value => value;
    }
}
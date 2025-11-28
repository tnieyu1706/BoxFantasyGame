using TnieCustomPackage.BackboneLogger;
using EditorAttributes;
using UnityEngine;

namespace _Project.Scripts.Test.Interface
{
    public interface IDataTest
    {
        void ShowInfo();
    }

    public class DataTest : MonoBehaviour, IDataTest
    {
        public string testValue;
        
        [Button]
        public void ShowInfo()
        {
        }
    }
}
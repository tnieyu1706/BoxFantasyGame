using Backbone;
using EditorAttributes;
using UnityEngine;
using Logger = Backbone.Logger;

namespace _Project.Scripts.Test.Interface
{
    public interface IDataTest
    {
        void ShowInfo();
    }

    public class DataTest : MonoBehaviour
    {
        [Button]
        private void ShowLog()
        {
            Logger.Log("Test log", LogLevel.Info, "Debug");
        }
    }
}
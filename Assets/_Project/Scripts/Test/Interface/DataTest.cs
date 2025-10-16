using UnityEngine;

namespace _Project.Scripts.Test.Interface
{
    public interface IDataTest
    {
        void ShowInfo();
    }

    public class DataTest : MonoBehaviour, IDataTest
    {
        public string value1;
        public int value2;
        public Vector3 value3;

        public void ShowInfo()
        {
            Debug.Log(value1);
        }
    }
}
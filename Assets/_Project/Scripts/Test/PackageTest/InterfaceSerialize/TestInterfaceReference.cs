using _Project.Scripts.Test.Interface;
using AYellowpaper;
using EditorAttributes;
using UnityEngine;

namespace _Project.Scripts.Test.PackageTest.InterfaceSerialize
{
    public class TestInterfaceReference : MonoBehaviour
    {
        public InterfaceReference<IDataTest, MonoBehaviour> testInterfaceReference;

        [Button]
        private void ButtonCheck()
        {
            Debug.Log($"Type: {testInterfaceReference.Value.GetType().Name}");
            if (testInterfaceReference != null && testInterfaceReference.Value != null)
            {
                testInterfaceReference.Value.ShowInfo();
            }
        }
    }
}
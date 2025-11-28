using _Project.Scripts.Test.Interface;
using AYellowpaper;
using EditorAttributes;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Test.PackageTest.InterfaceSerialize
{
    public class TestInterfaceReferenceProp : MonoBehaviour
    {
        public InterfaceReferenceProp<IDataTest, MonoBehaviour> testInterfaceReferenceProp;

        [Button]
        private void ButtonCheck()
        {
            Debug.Log($"Type: {testInterfaceReferenceProp.Value.GetType().Name}");
            if (testInterfaceReferenceProp != null && testInterfaceReferenceProp.Value != null)
            {
                testInterfaceReferenceProp.Value.ShowInfo();
            }
        }
    }
}
using AYellowpaper;
using EditorAttributes;
using UnityEngine;

namespace _Project.Test
{
    public class InterfaceRef : MonoBehaviour
    {
        
        public InterfaceReference<IObjectBehavior> interfaceReference;

        [Button]
        public void TestValue()
        {
            if (interfaceReference == null)
                return;
         
            Debug.Log($"interfaceReference: {interfaceReference.Value.Value}");
        }
        
    }
}
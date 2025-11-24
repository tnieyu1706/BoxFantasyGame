using AYellowpaper;
using EditorAttributes;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;

namespace _Project.Test
{
    public class InterfaceRef : MonoBehaviour
    {
        // public InterfaceReferenceGUIProp<IObjectBehavior> interfaceReferenceProp;
        public InterfaceReferenceGUI<IObjectBehavior> interfaceReference;
        

        // [Button]
        // public void TestValue()
        // {
        //     if (interfaceReferenceProp == null)
        //         return;
        //  
        //     Debug.Log($"interfaceReference: {interfaceReferenceProp.Value.Value}");
        // }
        
    }
}
using _Project.Scripts.SOAP.Event;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using UnityEngine;

namespace _Project.Scripts.SOAP.Test
{
    public class TestScript : MonoBehaviour
    {
        [PropertyDropdown]
        public SOAPEvent SoapEvent;

        void OnEnable()
        {
            SoapEvent.@event.AddListener(TestFunction);
        }

        void OnDisable()
        {
            SoapEvent.@event.RemoveListener(TestFunction);
        }

        public void TestFunction()
        {
            Debug.Log($"hello world! from {gameObject.name}");
        }
        
    }
}
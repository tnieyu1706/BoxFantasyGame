using System.Collections.Generic;
using _Project.Scripts.SOAP.Event;
using EditorAttributes;
using UnityEngine;

namespace _Project.Scripts.SOAP.Test
{
    public class TestScript : MonoBehaviour
    {
        [PropertyDropdown]
        public SoapEvent SoapEvent;

        [PropertyDropdown] public List<Transform> objectTransformList;

        [PropertyDropdown] public List<Component> components;

        public Component tnieDropdownComponent;

        [Validate("dataName not null!", nameof(CheckDataName))]
        public string dataName;

        private bool CheckDataName => string.IsNullOrEmpty(dataName);

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
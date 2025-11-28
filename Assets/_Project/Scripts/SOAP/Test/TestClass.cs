using System;
using UnityEngine;

namespace _Project.Scripts.SOAP.Test
{
    public interface ITestClass
    {
        string DefaultValue { get; }
    }
    
    [Serializable]
    public class TestClass : ITestClass
    {
        public string value1;
        public string value2;
        public float number1;
        [SerializeField] private string defaultValue;
        
        public string DefaultValue => defaultValue;
    }
}
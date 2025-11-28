using _Project.Test.TestSO;
using Cysharp.Text;
using EditorAttributes;
using NUnit.Framework;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.SOAP.Data;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace _Project.Test
{
    public class TestChangeStructComponent : MonoBehaviour
    {
        [TnieShowProperty(nameof(SValue))]
        public int sValueDummy;
        
        public string SValue => TestFunc(); //show only

        public string TestFunc()
        {
            return "hello xin chao";
        }

        [Button]
        private void TestLog(string message)
        {
            Logger.Log(message, category:"Debug");
        }
    }
}
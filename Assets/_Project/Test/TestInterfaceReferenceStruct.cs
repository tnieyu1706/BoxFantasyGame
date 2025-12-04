using System;
using TnieYuPackage.CustomAttributes;
using UnityEngine;

namespace _Project.Test
{
    public interface ITestStruct
    {
        public object Value { get; }
    }

    [Serializable]
    public struct TestStructAbstract : ITestStruct
    {
        [SerializeField] private int intValue;

        public object Value => intValue;
    }

    public class TestInterfaceReferenceStruct : MonoBehaviour
    {
        [SerializeReference, AbstractSupport(typeof(ITestStruct))]
        public ITestStruct testStructData;
    }
}
using _Project.Test.TestSO;
using Cysharp.Text;
using EditorAttributes;
using NUnit.Framework;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.ObjectDataSystem;
using TnieYuPackage.SOAP.Data;
using TnieYuPackage.Utils;
using TnieYuPackage.Utils.DictionaryUtil;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace _Project.Test
{
    public class TestChangeStructComponent : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionaryAbstract<SerializableGuid, IObjectData> datas;
    }
}
using UnityEngine;

namespace Systems.IdentifySystem.TypeIdentify
{
    public class TestIdentifyStorageFlagAttribute : TypeIdentifyStorageFlagAttribute
    {
        public new const string StorageIdentify = "Test";

        public TestIdentifyStorageFlagAttribute(string identify) : base(identify)
        {
        }
    }

    [CreateAssetMenu(fileName = "TestIdentifyStorage",
        menuName = "Scriptable Objects/IdentifyStorage/TestIdentifyStorage")]
    public class TestIdentifyStorage : TypeIdentifyStorage<TestIdentifyStorageFlagAttribute, TestIdentifyStorage>
    {
    }
}
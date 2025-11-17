using System.Collections.Generic;
using TnieYuPackage.Utils;

namespace Systems.IdentifySystem.ObjectIdentify
{
    public interface ICategoryIdentify
    {
        string CategoryName { get; }
    }

    public interface ICategoryIdentify<TObject> : ICategoryIdentify
        where TObject : IObjectIdentify
    {
        List<TObject> ObjectIdentifies { get; }

        Dictionary<SerializableGuid, TObject> GetMapById();
        Dictionary<string, TObject> GetMapByName();
    }
}
using TnieYuPackage.Utils;

namespace Systems.IdentifySystem.ObjectIdentify
{
    public interface IObjectIdentify
    {
        SerializableGuid Id { get; }
        string ObjectName { get; }
    }

    public interface IObjectIdentify<TCategory> : IObjectIdentify
        where TCategory : ICategoryIdentify
    {
        TCategory CategoryIdentify { get; }
    }
}
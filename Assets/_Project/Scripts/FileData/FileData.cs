using UnityEngine;

namespace Systems.FileData
{
    public abstract class FileData<TService> : ScriptableObject
        where TService : new()
    {
        public abstract string Path { get; }

        public TService service = new TService();
    }
}
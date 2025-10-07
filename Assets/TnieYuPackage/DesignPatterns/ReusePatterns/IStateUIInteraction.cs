using System;

namespace TnieYuPackage.DesignPatterns.ReusePatterns
{
    public interface IStateUIInteraction
    {
        public Action OnUIInitialized { get; }
        public Action OnUIClosed { get; }
    }
}
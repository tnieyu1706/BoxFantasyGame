using System.Collections.Generic;
using Systems.EntityDataSystem;

namespace Systems.ItemSystem
{
    /// <summary>
    /// IUsingProcedure is interface apply for ItemSystem
    /// with target => specify procedural working of item when using.
    /// And Current it only work with {Entity}
    /// </summary>
    public interface IUsingProcedure
    {
        void HandleProcedure(IItemUser itemUser, object target, List<IUsingEffect> effects);
    }
}
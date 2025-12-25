using UnityEngine;

namespace Systems.ItemSystem
{
    public interface IItemUser
    {
        Component User { get; }
    }
    
    public interface IUsingTargetTracking
    {
        object TrackTarget(IItemUser itemUser);
    }
}
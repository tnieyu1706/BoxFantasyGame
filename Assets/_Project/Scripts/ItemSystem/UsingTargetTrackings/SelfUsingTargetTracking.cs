using System;

namespace Systems.ItemSystem.UsingTargetTrackings
{
    [Serializable]
    public class SelfUsingTargetTracking : IUsingTargetTracking
    {
        public object TrackTarget(IItemUser itemUser)
        {
            return itemUser.User.gameObject;
        }
    }
}
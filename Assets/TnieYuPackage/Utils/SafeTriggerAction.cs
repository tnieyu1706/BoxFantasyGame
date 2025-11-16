using System;
using TnieYuPackage.GlobalExtensions;

namespace TnieYuPackage.Utils
{
    public class SafeTriggerAction : BaseTriggerAction
    {
        public override bool TryToAddTrigger(Func<bool> trigger)
        {
            if (ActionEvent.Contains(trigger)) return false;
            
            ActionEvent += trigger;
            return true;
        }
    }

    public class SafeTriggerAction<T1> : BaseTriggerAction<T1>
    {
        public override bool TryToAddTrigger(Func<T1, bool> trigger)
        {
            if (ActionEvent.Contains(trigger)) return false;
            
            ActionEvent += trigger;
            return true;
        }
    }
}
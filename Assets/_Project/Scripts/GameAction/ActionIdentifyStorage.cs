using System.Collections.Generic;
using Systems.IdentifySystem.TypeIdentify;
using TnieYuPackage.Utils;
using UnityEngine;

namespace Systems.GameAction
{
    public class ActionIdentifyStorageFlagAttribute : TypeIdentifyStorageFlagAttribute
    {
        public new const string StorageIdentify = "GlobalAction";
        
        public ActionIdentifyStorageFlagAttribute(string identify) : base(identify)
        {
            
        }
    }
    
    [CreateAssetMenu(fileName = "ActionIdentifyStorage", menuName = "Scriptable Objects/IdentifyStorage/ActionIdentifyStorage")]
    public class ActionIdentifyStorage : TypeIdentifyStorage<ActionIdentifyStorageFlagAttribute, ActionIdentifyStorage>
    {
        /// <summary>
        /// Only use in setup. it is data initialize depend on actionStorage keys.
        /// </summary>
        private Dictionary<string, SafeTriggerAction<GameActionCommandStaticDto>> loadEvents;

        /// <summary>
        /// Use to get dictionary loadEvents with ensure it always initialize data.
        /// </summary>
        public Dictionary<string, SafeTriggerAction<GameActionCommandStaticDto>> LoadEvents
        {
            get
            {
                if (loadEvents == null || loadEvents.Count != Datas.data.Count)
                {
                    InitializeLoadEvents();
                }
                
                return loadEvents;
            }
        }

        private void InitializeLoadEvents()
        {
            loadEvents = new();
            foreach (var dataKey in Datas.Dictionary.Keys)
            {
                loadEvents[dataKey] = new SafeTriggerAction<GameActionCommandStaticDto>();
            }
        }

        public bool TryToGetLoadEvent(string dataKey, out BaseTriggerAction<GameActionCommandStaticDto> triggerAction)
        {
            if (LoadEvents.TryGetValue(dataKey, out var safeTriggerAction))
            {
                triggerAction = safeTriggerAction;
                return true;
            }

            triggerAction = null;
            return false;
        }
    }
}
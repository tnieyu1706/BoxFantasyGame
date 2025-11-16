using System;
using UnityEngine;

namespace Systems.GeneralSystem.BackgroundEffectSystem
{
    [Serializable]
    public class LogBackgroundEffect : IBackgroundEffect
    {
        [TextArea(3, 10)] public string content;
        public void Perform()
        {
            Debug.Log("log: " + content);
        }
    }

    
}
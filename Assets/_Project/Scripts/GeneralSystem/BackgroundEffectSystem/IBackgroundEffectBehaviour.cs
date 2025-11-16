using System.Collections.Generic;

namespace Systems.GeneralSystem.BackgroundEffectSystem
{
    public interface IBackgroundEffectBehaviour
    {
        List<IBackgroundEffect> OpenEffects { get; }
        List<IBackgroundEffect> CloseEffects { get; }

        public void HandleOpenBackground()
        {
            if (OpenEffects == null || OpenEffects.Count == 0) return;
            OpenEffects.ForEach(e => e.Perform());
        }

        public void HandleCloseBackground()
        {
            if (CloseEffects == null || CloseEffects.Count == 0) return;
            CloseEffects.ForEach(e => e.Perform());
        }
    }
    
    public static class InterfaceBackgroundEffectBehaviourExtensions
    {
        public static void HandleOpenBackground(this IBackgroundEffectBehaviour behaviour)
        {
            behaviour.HandleOpenBackground();
        }

        public static void HandleCloseBackground(this IBackgroundEffectBehaviour behaviour)
        {
            behaviour.HandleCloseBackground();
        }
    }
    
}
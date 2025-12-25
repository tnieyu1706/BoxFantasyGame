using System.Collections.Generic;

namespace Systems.GeneralSystem.BackgroundEffectSystem
{
    public interface IBackgroundEffectBehaviour
    {
        List<IBackgroundEffect> OpenEffects { get; }
        List<IBackgroundEffect> CloseEffects { get; }

        void HandleOpenBackground();

        void HandleCloseBackground();
    }

    public interface IDefaultBackgroundEffectBehaviourMixin : IBackgroundEffectBehaviour
    {
        void IBackgroundEffectBehaviour.HandleOpenBackground()
        {
            if (OpenEffects == null || OpenEffects.Count == 0) return;
            OpenEffects.ForEach(e => e.Perform());
        }

        void IBackgroundEffectBehaviour.HandleCloseBackground()
        {
            if (CloseEffects == null || CloseEffects.Count == 0) return;
            CloseEffects.ForEach(e => e.Perform());
        }
    }
    
}
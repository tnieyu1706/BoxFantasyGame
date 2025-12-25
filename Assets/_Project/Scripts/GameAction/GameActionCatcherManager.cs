using System.Collections.Generic;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace Systems.GameAction
{
    public class GameActionCatcherManager : SingletonBehavior<GameActionCatcherManager>
    {
        private readonly List<IGameActionCatcher> catchers = new();

        public void Registry(IGameActionCatcher catchingData)
        {
            catchers.Add(catchingData);
        }

        public void UnRegistry(IGameActionCatcher catchingData)
        {
            catchers.Remove(catchingData);
        }

        public void Catching(GameActionCommand actionCommand)
        {
            //improve performance: improve coding (algorithm).
            List<IGameActionCatcher> completedCatchers = new();

            foreach (var catcher in catchers)
            {
                if (catcher.Format(actionCommand) && catcher.Catch(actionCommand))
                {
                    completedCatchers.Add(catcher);
                }
            }

            foreach (var completedCatcher in completedCatchers)
            {
                completedCatcher.OnCompleted?.Invoke();

                if (catchers.Contains(completedCatcher))
                {
                    catchers.Remove(completedCatcher);
                }
            }
        }
    }
}
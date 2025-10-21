using System.Collections.Generic;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace Systems.GameAction
{
    public class GameActionSystem : SingletonBehavior<GameActionSystem>
    {
        private Queue<GameActionCommand> queue = new();

        private GameActionCommand commandTrigger;

        public void PushCommand(GameActionCommand command)
        {
            queue.Enqueue(command);
        }

        void Update()
        {
            if (queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    commandTrigger = queue.Dequeue();
                    commandTrigger.Trigger();
                }
            }
        }
    }
}
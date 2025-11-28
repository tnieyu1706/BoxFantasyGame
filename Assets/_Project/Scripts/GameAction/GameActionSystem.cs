using System.Collections.Concurrent;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace Systems.GameAction
{
    public class GameActionSystem : SingletonBehavior<GameActionSystem>
    {
        private ConcurrentQueue<GameActionCommand> queue = new();

        private GameActionCommand commandTrigger;

        public void PushCommand(GameActionCommand command)
        {
            queue.Enqueue(command);
        }

        void Update()
        {
            while (queue.TryDequeue(out commandTrigger))
            {
                commandTrigger.Trigger();
            }
        }
    }
}
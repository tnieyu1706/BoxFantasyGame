using System.Collections.Concurrent;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace Systems.GameAction
{
    public class GameActionSystem : SingletonBehavior<GameActionSystem>
    {
        private ConcurrentQueue<GameActionCommand> queue = new();

        private GameActionCommand tempCommand;

        public void PushCommand(GameActionCommand command)
        {
            queue.Enqueue(command);
        }

        void Update()
        {
            while (queue.TryDequeue(out tempCommand))
            {
                tempCommand.Call();
                // more handle like: catching, tracking ...
                
                GameActionCatcherManager.Instance.Catching(tempCommand);
                
            }
        }
    }
}
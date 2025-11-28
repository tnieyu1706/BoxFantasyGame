using System;
using Cysharp.Threading.Tasks;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace _Project.Scripts.TaskManager
{
    public class WorkerTaskManager : SingletonBehavior<WorkerTaskManager>
    {
        public UniTask RunTaskImmediate(Action action)
        {
            return UniTask.RunOnThreadPool(action);
        }

        public async UniTask RunTaskDelayed(Action action, int delayMs)
        {
            await UniTask.Delay(delayMs);
            UniTask.RunOnThreadPool(action);
        }
    }
}
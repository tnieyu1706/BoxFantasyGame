using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.TaskManager
{
    public static class WorkerTaskManager
    {
        public static UniTask RunTaskImmediate(Action action)
        {
            return UniTask.RunOnThreadPool(action);
        }

        public static async UniTask RunTaskDelayed(Action action, int delayMs)
        {
            await UniTask.Delay(delayMs);
            UniTask.RunOnThreadPool(action);
        }
    }
}
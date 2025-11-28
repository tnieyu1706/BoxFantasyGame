using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace _Project.Scripts.TaskManager
{
    public class BackgroundJob
    {
        public UniTask task;
        public CancellationTokenSource cts;
        public BackgroundJob(UniTask task, CancellationTokenSource cts)
        {
            this.task = task;
            this.cts = cts;
        }
    }

    /// <summary>
    /// Singleton Component handle Looping task
    /// Looping task: run start - end.
    /// </summary>
    public class BackgroundTaskManager : SingletonBehavior<BackgroundTaskManager>
    {
        private Dictionary<Guid, BackgroundJob> tasks = new();

        private async UniTask GenerateBackgroundTask(Action action, int delayMs, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                action?.Invoke();
                await UniTask.Delay(delayMs, cancellationToken: token);
            }
        }

        /// <summary>
        /// Registry task want to become looping.
        /// Ensure action don't occur garbage variables.
        /// </summary>
        /// <param name="action">Method run once</param>
        /// <param name="delayMs">delay milliseconds</param>
        /// <returns></returns>
        public Guid RegistryBgTask(Action action, int delayMs)
        {
            CancellationTokenSource cts = new();
            UniTask task = GenerateBackgroundTask(action, delayMs, cts.Token);
            BackgroundJob bgJob = new(task, cts);

            Guid taskId = Guid.NewGuid();
            tasks.Add(taskId, bgJob);

            return taskId;
        }

        /// <summary>
        /// Try to get taskId to cancel CancellationSource
        /// </summary>
        /// <param name="taskId">Task Guid id</param>
        /// <returns></returns>
        public bool StopBgTask(Guid taskId)
        {
            if (tasks.ContainsKey(taskId))
            {
                tasks[taskId].cts.Cancel();
                tasks.Remove(taskId);

                return true;
            }

            return false;
        }
    }
}
using System;
using System.Collections.Concurrent;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;

namespace _Project.Scripts.TaskManager
{
    /// <summary>
    /// Singleton Component to handle Unity API method
    /// </summary>
    public class UnityTaskManager : SingletonBehavior<UnityTaskManager>
    {
        private ConcurrentQueue<Action> taskQueue = new();

        void Update()
        {
            while (taskQueue.TryDequeue(out var task))
            {
                task?.Invoke();
            }
        }

        /// <summary>
        /// Handle Unity API Method
        /// </summary>
        /// <param name="action">Unity API</param>
        public void RegistryUnityTask(Action action)
        {
            taskQueue.Enqueue(action);
        }
        
    }
}
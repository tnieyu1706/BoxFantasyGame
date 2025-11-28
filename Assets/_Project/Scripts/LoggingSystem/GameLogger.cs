using System;
using System.Collections.Generic;
using _Project.Scripts.TaskManager;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;
using Logger = TnieCustomPackage.BackboneLogger.Logger;

namespace _Project.Scripts.LoggingSystem
{
    public class GameLogger : SceneSingletonBehaviour<GameLogger>
    {
        #region Properties

        [SerializeField] GameObject logElementPrefab;
        [SerializeField] Transform logParent;
        [SerializeField] int maxLogElements;

        [SerializeField] float displaySeconds = 1.5f;
        [SerializeField] private int loggingDelayMs = 200;

        #endregion

        #region Data

        private class LogStackData
        {
            public readonly LogElement Log;
            public int Stack = 0;
            public float TimeLife;
            private readonly float defaultTimeLife;

            public LogStackData(LogElement log, float timeLife)
            {
                this.Log = log;
                this.TimeLife = timeLife;
                this.defaultTimeLife = timeLife;
            }

            public void SetTimeLife(float time)
            {
                TimeLife = time;
            }

            public void ResetTimeLife()
            {
                TimeLife = defaultTimeLife;
            }
        }

        private LogElement[] logElements;
        private readonly Dictionary<string, LogStackData> logStack = new();
        private int lastGetLogIndex = 0;

        private LogStackData logDataTemp;
        private readonly List<string> removingStack = new();

        private Guid loggerBgTaskId = Guid.Empty;

        #endregion

        protected override void Awake()
        {
            base.Awake();

            InitializeLogElements();
        }

        private void OnEnable()
        {
            loggerBgTaskId = BackgroundTaskManager.Instance.RegistryBgTask(Updating, loggingDelayMs);
        }

        private void OnDisable()
        {
            if (loggerBgTaskId == Guid.Empty)
            {
                Debug.LogWarning("Game Logger Background Task not found.");
                return;
            }

            if (BackgroundTaskManager.Instance != null)
                if (BackgroundTaskManager.Instance.StopBgTask(loggerBgTaskId))
                {
                    Logger.Log("Completed stop logger background task.", category: "Runtime");
                }
        }

        /// <summary>
        /// Auto handle logStack expire & timeLife.
        /// </summary>
        void Updating()
        {
            if (logStack.Count < 0) return;

            removingStack.Clear();
            foreach (var log in logStack)
            {
                log.Value.TimeLife -= (float)loggingDelayMs / 1000;

                if (log.Value.TimeLife <= 0)
                {
                    removingStack.Add(log.Key);
                    UnityTaskManager.Instance
                        .RegistryUnityTask(() => log.Value.Log.gameObject.SetActive(false));
                }
            }

            if (removingStack.Count > 0)
            {
                removingStack.ForEach(r => logStack.Remove(r));
            }
        }

        #region Functions

        private void InitializeLogElements()
        {
            logElements ??= new LogElement[maxLogElements];
            LogElement logElement;
            for (int i = 0; i < maxLogElements; i++)
            {
                logElement = Instantiate(logElementPrefab, logParent).GetComponent<LogElement>();
                logElement.gameObject.SetActive(false);

                logElements[i] = logElement;
            }

            Logger.Log("Initialized log elements", category: "UI");
        }

        public void Log(string key, string message)
        {
            //Get & Show message.
            LogElement logElement;

            if (logStack.TryGetValue(key, out logDataTemp))
            {
                logElement = logDataTemp.Log;
                message += $" {++logDataTemp.Stack}";

                logDataTemp.ResetTimeLife();
            }
            else
            {
                logElement = GetLogElement();
                logStack[key] = new LogStackData(logElement, displaySeconds);
            }

            logElement.text.SetText(message);
            logElement.gameObject.SetActive(true);
        }

        /// <summary>
        /// Get conforming LogElement
        /// </summary>
        /// <returns></returns>
        LogElement GetLogElement()
        {
            int nextLogIndex = lastGetLogIndex + 1;
            if (nextLogIndex > maxLogElements - 1)
                nextLogIndex %= maxLogElements;

            lastGetLogIndex = nextLogIndex;

            return logElements[nextLogIndex];
        }

        #endregion
    }
}
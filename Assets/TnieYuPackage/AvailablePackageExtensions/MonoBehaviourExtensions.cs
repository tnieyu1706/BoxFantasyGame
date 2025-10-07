using System;
using System.Collections;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace TnieYuPackage.AvailablePackageExtensions
{
    public static class MonoBehaviourExtensions
    {
        public static void Invoke(this MonoBehaviour monoBehaviour, Action action, float delay)
        {
            monoBehaviour.StartCoroutine(InvokeCoroutine(action, delay));
        }
    
        public static IEnumerator InvokeCoroutine(Action action, float delay)
        {
            yield return SingletonFactory.GetInstance<WaitForSecondsServiceLocator>().GetService(delay);
        
            action?.Invoke();
        }
    }
}
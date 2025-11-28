using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace Systems.Input
{
    public interface IInputReader
    {
        void EnableActions();
        Action<string> OnInputSubscribe { get; set; }
        Action<string> OnInputUnsubscribe { get; set; }

        public static InputSystem InputSystem;
    }

    public static class InterfaceInputReaderExtensions
    {
        public static List<IInputInfo> GetInputInfos(this IInputReader inputReader)
        {
            var currentType = inputReader.GetType();

            var inputFields =
                currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => typeof(IInputInfo).IsAssignableFrom(f.FieldType));

            return inputFields.Select(f => (IInputInfo)f.GetValue(inputReader)).ToList();
        }
    }

    public abstract class BaseInputReader<T> : SingletonScriptable<T>, IInputReader
        where T : BaseInputReader<T>
    {
        public Action<string> OnInputSubscribe { get; set; }
        public Action<string> OnInputUnsubscribe { get; set; }

        [Button]
        private void SetupInputInfos()
        {
            var inputs = this.GetInputInfos();
            foreach (var i in inputs)
            {
                i.InputReader = this;
            }

            Debug.Log($"Have setup for {inputs.Count} InputInfos");
        }

        public abstract void EnableActions();
    }
}
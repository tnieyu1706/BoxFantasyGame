using System;
using AYellowpaper;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Input
{
    public interface IInputInfo
    {
        string InputName { get; set; }
        Sprite InputIcon { get; set; }
        IInputReader InputReader { get; set; }
    }

    [Serializable]
    public struct InputInfo<TEvent> : IInputInfo
        where TEvent : Delegate
    {
        [SerializeField] private string inputName;
        [SerializeField] private Sprite inputIcon;
        [SerializeField] private InterfaceReference<IInputReader, ScriptableObject> inputReader;
        [SerializeField] private bool toggle;
        private TEvent @event;

        public TEvent Event
        {
            get => @event;
            set
            {
                if (!toggle)
                {
                    @event = value;
                    return;
                }
                
                bool wasEmpty = @event == null;
                bool willBeEmpty = value == null;
                
                @event = value;
                
                if (wasEmpty && value != null)
                {
                    InputReader.OnInputSubscribe?.Invoke(InputName);
                }
                if (!wasEmpty && willBeEmpty)
                {
                    InputReader.OnInputUnsubscribe?.Invoke(InputName);
                }
            }
        }

        public string InputName
        {
            get => inputName;
            set => inputName = value;
        }

        public Sprite InputIcon
        {
            get => inputIcon;
            set => inputIcon = value;
        }

        public IInputReader InputReader
        {
            get => inputReader.Value;
            set => inputReader.Value = value;
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using AYellowpaper;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Void = EditorAttributes.Void;

namespace Systems.SOAP.Data
{
    public abstract class SOAPData<T> : ScriptableObject
    {
        [SerializeField] [SetProperty(nameof(Value))]
        private T value;

        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        [SerializeField] private bool resetOnPlay = false;

        [SerializeField, ShowField(nameof(resetOnPlay)), IndentProperty]
        private T defaultValue;

        [PropertyOrder(9)] public UnityEvent<T> OnValueChanged;

        #region ValueChangeManual

        [SerializeField, HideInInspector] private bool changeValueInInspector = true;

        [SerializeField, HideInInspector, ShowField(nameof(changeValueInInspector)), IndentProperty]
        private T valueChange;

        [SerializeField, HideInInspector, ShowField(nameof(changeValueInInspector)),
         ButtonField(nameof(ChangeValueManual)), IndentProperty]
        private Void raiseEventButton;

        [FoldoutGroup(
            "Change Value",
            nameof(changeValueInInspector),
            nameof(valueChange),
            nameof(raiseEventButton)
        )]
        public Void changeValueFoldoutGroup;

        public void ChangeValueManual()
        {
            if (!changeValueInInspector)
                return;

            Value = valueChange;
            Debug.Log("Value Changed!");
        }

        #endregion

        protected virtual void OnDisable()
        {
            if (resetOnPlay)
            {
                Value = defaultValue;
            }
        }
    }

    public abstract class SOAPInterfaceData<T> : ScriptableObject
        where T : class
    {
        [SerializeField] [SetProperty(nameof(Value))]
        private InterfaceReference<T, Object> valueReference;

        public T Value
        {
            get => valueReference.Value;
            set
            {
                OnValueChanged?.Invoke(value);
                valueReference.Value = value;
            }
        }

        public UnityEvent<T> OnValueChanged;
    }
    
}
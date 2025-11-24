using EditorAttributes;
using Amirebrahimi.SetProperty.Scripts;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace TnieYuPackage.SOAP.Data
{
    public abstract class SoapData<T> : ScriptableObject
    {
        [SerializeField] [SetProperty(nameof(Value))]
        private T value;

        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChanged?.Invoke(value);
            }
        }

        [SerializeField] private bool resetOnPlay = false;

        [SerializeField, ShowField(nameof(resetOnPlay)), IndentProperty]
        private T defaultValue;

        public UnityEvent<T> onValueChanged;

        protected virtual void OnDisable()
        {
            if (resetOnPlay)
            {
                Value = defaultValue;
            }
        }
    }

    public abstract class SoapInterfaceData<T> : ScriptableObject
        where T : class
    {
        [FormerlySerializedAs("valueReference")] [SerializeField] [SetProperty(nameof(Value))]
        private InterfaceReferenceProp<T, Object> valueReferenceProp;

        public T Value
        {
            get => valueReferenceProp.Value;
            set
            {
                onValueChanged?.Invoke(value);
                valueReferenceProp.Value = value;
            }
        }

        public UnityEvent<T> onValueChanged;
    }
    
}
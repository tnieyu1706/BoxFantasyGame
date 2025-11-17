using System;
using System.Reflection;
using UnityEngine;

namespace Systems.GeneralSystem.ObjectDataSystem
{
    [Serializable]
    public abstract class ObjectData : IComparable<ObjectData>, ICloneable
    {
        /// <summary>
        /// Use to see Set | Get depend on Set/GetMethod instead.
        /// </summary>
        public object value;

        /// <summary>
        /// Use Value property to get actual value (from child)
        /// </summary>
        public object Value
        {
            get
            {
                var valueField = GetValueField();

                if (valueField == null)
                {
                    Debug.LogWarning("Cannot get value field!");
                    return null;
                }

                return valueField.GetValue(this);
            }

            set
            {
                if (!ValidateData(this, value))
                {
                    Debug.LogWarning(
                        $"Current ObjectDataType {this.GetType().FullName} not valid with valueType {value.GetType().FullName}");
                    return;
                }

                var valueField = GetValueField();

                if (valueField == null)
                {
                    Debug.LogWarning("Cannot set value field!");
                    return;
                }

                valueField.SetValue(this, value);
            }
        }

        private FieldInfo GetValueField()
        {
            return this.GetType().GetField(
                nameof(value),
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public
            );
        }

        public static void InitializeTypeData(FieldType fieldType, out ObjectData objectData)
        {
            objectData = fieldType switch
            {
                FieldType.Bool => new BooleanData(),
                FieldType.Int => new IntData(),
                FieldType.String => new StringData(),
                _ => null
            };
        }

        public static bool ValidateData(ObjectData objectData, object value) =>
            objectData.Value.GetType() == value.GetType();

        public static bool ValidateData(ObjectData objectData, ObjectData otherData) =>
            objectData.Value.GetType() == otherData.Value.GetType();

        public static void SetValueDirectly(FieldType fieldType, object value, out ObjectData objectData)
        {
            InitializeTypeData(fieldType, out objectData);
            objectData.Value = value;
        }

        public int CompareTo(ObjectData other)
        {
            if (ValidateData(this, other))
            {
                if (this.Value is IComparable thisComparable && other.Value is IComparable otherComparable)
                {
                    return thisComparable.CompareTo(otherComparable);
                }
            }

            throw new ArgumentException("Object data is not valid");
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectData objectData)
            {
                return ValidateData(this, objectData) && objectData.CompareTo(this) == 0;
            }
            
            return false;
        }

        protected bool Equals(ObjectData other)
        {
            return Equals(value, other.value);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public object Clone()
        {
            var valueType = this.GetType();
            
            ObjectData objectClone = (ObjectData)Activator.CreateInstance(valueType);
            var valueField = objectClone.GetValueField();
            if (valueField != null)
            {
                var currentValue = valueField.GetValue(this);

                if (currentValue is ICloneable cloneableValue)
                {
                    Debug.Log("Clone");
                    currentValue = cloneableValue.Clone();
                }

                valueField.SetValue(objectClone, currentValue);
            }

            return objectClone;
        }

        public static ObjectData operator +(ObjectData first, ObjectData second)
        {
            if (!ValidateData(first, second))
            {
                Debug.LogWarning("first and second objects are not valid");
                return first;
            }

            switch (first)
            {
                case IntData firstInt:
                    firstInt.value += ((IntData)second).value;
                    return firstInt;
                
                case FloatData firstFloat:
                    firstFloat.value += ((FloatData)second).value;
                    return firstFloat;
                
                case StringData firstString:
                    firstString.value += ((StringData)second).value;
                    return firstString;
                
                default:
                    return first;
            }
        }
        
        public static ObjectData operator -(ObjectData first, ObjectData second)
        {
            if (!ValidateData(first, second))
            {
                Debug.LogWarning("first and second objects are not valid");
                return first;
            }

            switch (first)
            {
                case IntData firstInt:
                    firstInt.value -= ((IntData)second).value;
                    return firstInt;
                
                case FloatData firstFloat:
                    firstFloat.value -= ((FloatData)second).value;
                    return firstFloat;
                
                default:
                    return first;
            }
        }
    }

    [Serializable]
    public abstract class ObjectData<T> : ObjectData
    {
        public new T value;
    }

    [Serializable]
    public class BooleanData : ObjectData<bool>
    {
    }

    [Serializable]
    public class IntData : ObjectData<int>
    {
    }

    [Serializable]
    public class StringData : ObjectData<string>
    {
    }

    [Serializable]
    public class FloatData : ObjectData<float>
    {
        
    }
}
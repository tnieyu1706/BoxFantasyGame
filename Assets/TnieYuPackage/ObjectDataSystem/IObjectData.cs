using System;

namespace TnieYuPackage.ObjectDataSystem
{
    /// <summary>
    /// IObjectData is a nature for object with data custom like: Int, Float, Boolean, String (defaultData).
    /// using [struct] for structure so it's a immutable object.
    /// </summary>
    public interface IObjectData
    {
        /// <summary>
        /// Use to see Set | Get depend on Set/GetMethod instead.
        /// </summary>
        object Value { get; set; }

        bool IsValid(IObjectData other);
        bool IsValidValue(object otherValue);

        #region SELF OVERRIDE

        IObjectData Add(IObjectData other);
        IObjectData Add(object otherValue);

        IObjectData Minus(IObjectData other);
        IObjectData Minus(object otherValue);

        #endregion

        bool Equals(IObjectData other);
        bool Equals(object otherValue);

        bool GreaterThan(IObjectData other);
        bool GreaterThan(object otherValue);

        bool GreaterThanOrEqual(IObjectData other);
        bool GreaterThanOrEqual(object otherValue);
    }

    public interface IObjectData<T> : IObjectData
        where T : IComparable
    {
        object IObjectData.Value
        {
            get => ActualValue;
            set
            {
                if (IsValidValue(value))
                    ActualValue = (T)value;
            }
        }

        T ActualValue { get; set; }

        Type GetCurrentType();

        bool IObjectData.IsValid(IObjectData other) => GetCurrentType().IsInstanceOfType(other);
        bool IObjectData.IsValidValue(object otherValue) => otherValue is T;

        bool IObjectData.Equals(IObjectData other) => IsValid(other) && Value.Equals(other.Value);
        bool IObjectData.Equals(object otherValue) => IsValidValue(otherValue) && Value.Equals(otherValue);

        bool IObjectData.GreaterThan(IObjectData other) => IsValid(other) && ActualValue.CompareTo(other.Value) > 0;

        bool IObjectData.GreaterThan(object otherValue) =>
            IsValidValue(otherValue) && ActualValue.CompareTo(otherValue) > 0;

        bool IObjectData.GreaterThanOrEqual(IObjectData other) =>
            IsValid(other) && ActualValue.CompareTo(other.Value) >= 0;

        bool IObjectData.GreaterThanOrEqual(object otherValue) =>
            IsValidValue(otherValue) && ActualValue.CompareTo(otherValue) >= 0;
    }

    public static class InterfaceObjectDataExtensions
    {
        public static bool IsValid(this IObjectData objectData, IObjectData other)
        {
            return objectData.IsValid(other);
        }

        public static bool IsValidValue(this IObjectData objectData, object otherValue)
        {
            return objectData.IsValidValue(otherValue);
        }
    }
}
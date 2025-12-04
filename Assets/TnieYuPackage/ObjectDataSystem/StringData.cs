using System;
using UnityEngine;

namespace TnieYuPackage.ObjectDataSystem
{
    [Serializable]
    public struct StringData : IObjectData<string>
    {
        [SerializeField] private string value;

        public Type GetCurrentType() => typeof(StringData);

        public string ActualValue
        {
            get => value;
            set => this.value = value;
        }

        public IObjectData Add(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value += ((StringData)other).value;
            return this;
        }

        public IObjectData Add(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value += (string)otherValue;
            return this;
        }

        public IObjectData Minus(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            string otherString = ((StringData)other).value;
            if (value.Contains(value))
            {
                value = value.Replace(otherString, "", StringComparison.Ordinal);
            }

            return this;
        }

        public IObjectData Minus(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            string otherString = (string)otherValue;
            if (value.Contains(value))
            {
                value = value.Replace(otherString, "", StringComparison.Ordinal);
            }

            return this;
        }
    }
}
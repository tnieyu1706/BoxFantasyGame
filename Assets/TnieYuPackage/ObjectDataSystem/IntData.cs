using System;
using UnityEngine;

namespace TnieYuPackage.ObjectDataSystem
{
    [Serializable]
    public struct IntData : IObjectData<int>
    {
        [SerializeField] private int value;

        public Type GetCurrentType() => typeof(IntData);

        public int ActualValue
        {
            get => value;
            set => this.value = value;
        }

        public IObjectData Add(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value += ((IntData)other).value;

            return this;
        }

        public IObjectData Add(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value += (int)otherValue;
            return this;
        }

        public IObjectData Minus(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value -= ((IntData)other).value;

            return this;
        }

        public IObjectData Minus(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value -= (int)otherValue;
            return this;
        }
    }
}
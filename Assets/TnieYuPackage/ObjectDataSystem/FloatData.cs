using System;
using UnityEngine;

namespace TnieYuPackage.ObjectDataSystem
{
    [Serializable]
    public struct FloatData : IObjectData<float>
    {
        [SerializeField] private float value;
        private IObjectData objectDataImplementation;

        public Type GetCurrentType() => typeof(FloatData);

        public float ActualValue
        {
            get => value;
            set => this.value = value;
        }

        public IObjectData Add(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value += ((FloatData)other).value;

            return this;
        }

        public IObjectData Add(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value += (float)otherValue;
            return this;
        }

        public IObjectData Minus(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value -= ((FloatData)other).value;

            return this;
        }

        public IObjectData Minus(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value -= (float)otherValue;
            return this;
        }
    }
}
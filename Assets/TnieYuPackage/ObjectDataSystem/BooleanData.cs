using System;
using UnityEngine;

namespace TnieYuPackage.ObjectDataSystem
{
    [Serializable]
    public struct BooleanData : IObjectData<bool>
    {
        [SerializeField] private bool value;

        public Type GetCurrentType() => typeof(BooleanData);

        public bool ActualValue
        {
            get => value;
            set => this.value = value;
        }

        public IObjectData Add(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value = ((BooleanData)other).value || value;
            return this;
        }

        public IObjectData Add(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value = (bool)otherValue || value;
            return this;
        }

        public IObjectData Minus(IObjectData other)
        {
            if (!this.IsValid(other)) return this;

            value = !((BooleanData)other).value && value;
            return this;
        }

        public IObjectData Minus(object otherValue)
        {
            if (!this.IsValidValue(otherValue)) return this;

            value = !(bool)otherValue && value;
            return this;
        }
    }
}
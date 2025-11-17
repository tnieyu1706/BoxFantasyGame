using System;
using System.Collections.Generic;

namespace Systems.GeneralSystem.ObjectDataSystem
{
    public enum FieldType
    {
        Int,
        Bool,
        String
    }

    public class FieldTypeData
    {
        public Type Type;

        public FieldTypeData(Type type)
        {
            this.Type = type;
        }
    }

    public static class FieldTypeSupport
    {
        public static readonly Dictionary<FieldType, FieldTypeData> FieldTypes = new();

        static FieldTypeSupport()
        {
            FieldTypes[FieldType.Int] = new FieldTypeData(typeof(int));
            FieldTypes[FieldType.Bool] = new FieldTypeData(typeof(bool));
            FieldTypes[FieldType.String] = new FieldTypeData(typeof(string));
        }
    }
}
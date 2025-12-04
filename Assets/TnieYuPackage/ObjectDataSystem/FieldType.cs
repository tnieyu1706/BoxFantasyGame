using System;
using System.Collections.Generic;

namespace TnieYuPackage.ObjectDataSystem
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
        public Func<IObjectData> ConvertToObjectDataProcedure;

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
            FieldTypes[FieldType.Int] = new FieldTypeData(typeof(int))
            {
                ConvertToObjectDataProcedure = () => new IntData()
            };
            
            FieldTypes[FieldType.Bool] = new FieldTypeData(typeof(bool))
            {
                ConvertToObjectDataProcedure = () => new BooleanData()
            };
            
            FieldTypes[FieldType.String] = new FieldTypeData(typeof(string))
            {
                ConvertToObjectDataProcedure = () => new StringData()
            };
        }
    }
}
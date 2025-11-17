using System;
using System.Collections.Generic;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Generals
{
    public enum CompareOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanEqual,
        LessThan,
        LessThanEqual,
    }

    public class CompareOperatorData
    {
        public string DisplayName;
        public Func<IComparable, IComparable, bool> Compare;

        public CompareOperatorData(string displayName, Func<IComparable, IComparable, bool> compare)
        {
            DisplayName = displayName;
            Compare = compare;
        }
    }

    public static class CompareOperatorSupport
    {
        public static readonly Dictionary<CompareOperator, CompareOperatorData> Operators = new();

        static CompareOperatorSupport()
        {
            Operators[CompareOperator.Equals] = new CompareOperatorData(
                "==",
                (a, b) => a.Equals(b)
            );

            Operators[CompareOperator.NotEquals] = new CompareOperatorData(
                "!=",
                (a, b) => !a.Equals(b)
            );

            Operators[CompareOperator.GreaterThan] = new CompareOperatorData(
                ">",
                (a, b) => a.CompareTo(b) > 0
            );

            Operators[CompareOperator.GreaterThanEqual] = new CompareOperatorData(
                ">=",
                (a, b) => a.CompareTo(b) >= 0
            );

            Operators[CompareOperator.LessThan] = new CompareOperatorData(
                "<",
                (a, b) => a.CompareTo(b) < 0
            );

            Operators[CompareOperator.LessThanEqual] = new CompareOperatorData(
                "<=",
                (a, b) => a.CompareTo(b) <= 0
            );
        }
    }
}
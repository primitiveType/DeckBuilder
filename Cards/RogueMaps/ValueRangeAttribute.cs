using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the integral value falls into the specified interval.
    ///     It's allowed to specify multiple non-intersecting intervals.
    ///     Values of interval boundaries are inclusive.
    /// </summary>
    /// <example>
    ///     <code>
    /// void Foo([ValueRange(0, 100)] int value) {
    ///   if (value == -1) { // Warning: Expression is always 'false'
    ///     ...
    ///   }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Method | AttributeTargets.Delegate,
        AllowMultiple = true)]
    public sealed class ValueRangeAttribute : Attribute
    {
        public ValueRangeAttribute(long from, long to)
        {
            From = from;
            To = to;
        }

        public ValueRangeAttribute(ulong from, ulong to)
        {
            From = from;
            To = to;
        }

        public ValueRangeAttribute(long value)
        {
            From = To = value;
        }

        public ValueRangeAttribute(ulong value)
        {
            From = To = value;
        }

        public object From { get; }
        public object To { get; }
    }
}

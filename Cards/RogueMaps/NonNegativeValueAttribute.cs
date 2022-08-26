using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the integral value never falls below zero.
    /// </summary>
    /// <example>
    ///     <code>
    /// void Foo([NonNegativeValue] int value) {
    ///   if (value == -1) { // Warning: Expression is always 'false'
    ///     ...
    ///   }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Method | AttributeTargets.Delegate)]
    public sealed class NonNegativeValueAttribute : Attribute
    {
    }
}

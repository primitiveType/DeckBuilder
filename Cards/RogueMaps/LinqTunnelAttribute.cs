using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the method is a pure LINQ method, with postponed enumeration (like Enumerable.Select,
    ///     .Where). This annotation allows inference of [InstantHandle] annotation for parameters
    ///     of delegate type by analyzing LINQ method chains.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class LinqTunnelAttribute : Attribute
    {
    }
}

using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the return value of the method invocation must be used.
    /// </summary>
    /// <remarks>
    ///     Methods decorated with this attribute (in contrast to pure methods) might change state,
    ///     but make no sense without using their return value. <br />
    ///     Similarly to <see cref="PureAttribute" />, this attribute
    ///     will help to detect usages of the method when the return value is not used.
    ///     Optionally, you can specify a message to use when showing warnings, e.g.
    ///     <code>[MustUseReturnValue("Use the return value to...")]</code>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MustUseReturnValueAttribute : Attribute
    {
        public MustUseReturnValueAttribute()
        {
        }

        public MustUseReturnValueAttribute([NotNull] string justification)
        {
            Justification = justification;
        }

        [CanBeNull] public string Justification { get; }
    }
}

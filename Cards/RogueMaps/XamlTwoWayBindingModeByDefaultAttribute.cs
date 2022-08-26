using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     XAML attribute. Indicates that DependencyProperty has <c>TwoWay</c> binding mode by default.
    /// </summary>
    /// <remarks>
    ///     This attribute must be applied to DependencyProperty's CLR accessor property if it is present, to
    ///     DependencyProperty descriptor field otherwise.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class XamlTwoWayBindingModeByDefaultAttribute : Attribute
    {
    }
}

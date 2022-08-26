﻿using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     XAML attribute. Indicates the property of some <c>Style</c>-derived type, that
    ///     is used to style items of <c>ItemsControl</c>-derived type. This annotation will
    ///     enable the <c>DataContext</c> type resolve for XAML bindings for such properties.
    /// </summary>
    /// <remarks>
    ///     Property should have the tree ancestor of the <c>ItemsControl</c> type or
    ///     marked with the <see cref="XamlItemsControlAttribute" /> attribute.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class XamlItemStyleOfItemsControlAttribute : Attribute
    {
    }
}

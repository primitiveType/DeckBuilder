using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the marked parameter, field, or property is a route template.
    /// </summary>
    /// <remarks>
    ///     This attribute allows IDE to recognize the use of web frameworks' route templates
    ///     to enable syntax highlighting, code completion, navigation, rename and other features in string literals.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class RouteTemplateAttribute : Attribute
    {
    }
}

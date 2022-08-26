using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the marked parameter is a message template where placeholders are to be replaced by the following
    ///     arguments
    ///     in the order in which they appear
    /// </summary>
    /// <example>
    ///     <code>
    /// void LogInfo([StructuredMessageTemplate]string message, params object[] args) { /* do something */ }
    /// 
    /// void Foo() {
    ///   LogInfo("User created: {username}"); // Warning: Non-existing argument in format string
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class StructuredMessageTemplateAttribute : Attribute
    {
    }
}

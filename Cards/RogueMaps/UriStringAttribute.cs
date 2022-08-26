using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the marked parameter, field, or property is an URI string.
    /// </summary>
    /// <remarks>
    ///     This attribute enables code completion, navigation, rename and other features
    ///     in URI string literals assigned to annotated parameter, field or property.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class UriStringAttribute : Attribute
    {
        public UriStringAttribute()
        {
        }

        public UriStringAttribute(string httpVerb)
        {
            HttpVerb = httpVerb;
        }

        [CanBeNull] public string HttpVerb { get; }
    }
}

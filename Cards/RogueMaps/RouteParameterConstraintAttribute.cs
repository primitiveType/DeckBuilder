using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     Indicates that the marked type is custom route parameter constraint,
    ///     which is registered in application's Startup with name <c>ConstraintName</c>
    /// </summary>
    /// <remarks>
    ///     You can specify <c>ProposedType</c> if target constraint matches only route parameters of specific type,
    ///     it will allow IDE to create method's parameter from usage in route template
    ///     with specified type instead of default <c>System.String</c>
    ///     and check if constraint's proposed type conflicts with matched parameter's type
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RouteParameterConstraintAttribute : Attribute
    {
        public RouteParameterConstraintAttribute([NotNull] string constraintName)
        {
            ConstraintName = constraintName;
        }

        [NotNull] public string ConstraintName { get; }
        [CanBeNull] public Type ProposedType { get; set; }
    }
}

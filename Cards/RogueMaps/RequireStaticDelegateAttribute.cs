using System;

namespace RogueMaps.Annotations
{
    /// <summary>
    ///     This annotation allows to enforce allocation-less usage patterns of delegates for performance-critical APIs.
    ///     When this annotation is applied to the parameter of delegate type, IDE checks the input argument of this parameter:
    ///     * When lambda expression or anonymous method is passed as an argument, IDE verifies that the passed closure
    ///     has no captures of the containing local variables and the compiler is able to cache the delegate instance
    ///     to avoid heap allocations. Otherwise the warning is produced.
    ///     * IDE warns when method name or local function name is passed as an argument as this always results
    ///     in heap allocation of the delegate instance.
    /// </summary>
    /// <remarks>
    ///     In C# 9.0 code IDE would also suggest to annotate the anonymous function with 'static' modifier
    ///     to make use of the similar analysis provided by the language/compiler.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RequireStaticDelegateAttribute : Attribute
    {
        public bool IsError { get; set; }
    }
}

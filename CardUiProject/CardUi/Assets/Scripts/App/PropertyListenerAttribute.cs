using System;
using JetBrains.Annotations;

namespace App
{
    [MeansImplicitUse]
    public class PropertyListenerAttribute : Attribute
    {
        public readonly string NameFilter;

        public PropertyListenerAttribute(string nameFilter = null)
        {
            NameFilter = nameFilter;
        }
    }
}
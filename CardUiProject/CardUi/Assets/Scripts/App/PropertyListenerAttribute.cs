using System;
using JetBrains.Annotations;

namespace Common
{
    [MeansImplicitUse]
    public class PropertyListenerAttribute : Attribute
    {
        public readonly string m_NameFilter;

        public PropertyListenerAttribute(string nameFilter = null)
        {
            m_NameFilter = nameFilter;
        }
    }
}
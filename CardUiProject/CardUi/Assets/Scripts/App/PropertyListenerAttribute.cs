using System;

namespace Common
{
    public class PropertyListenerAttribute : Attribute
    {
        public readonly string m_NameFilter;

        public PropertyListenerAttribute(string nameFilter = null)
        {
            m_NameFilter = nameFilter;
        }
    }
}
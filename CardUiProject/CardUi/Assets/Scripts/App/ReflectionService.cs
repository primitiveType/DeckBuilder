using System;
using System.Collections.Generic;
using System.Reflection;

namespace App
{
    public static class ReflectionService
    {
        private static readonly Dictionary<Type, List<PropertyListenerInfo>> PropertyListeners =
            new Dictionary<Type, List<PropertyListenerInfo>>();

        public static List<PropertyListenerInfo> GetPropertyListeners(Type type)
        {
            if (PropertyListeners.ContainsKey(type))
            {
                return PropertyListeners[type];
            }

            List<PropertyListenerInfo> methods = new List<PropertyListenerInfo>();
            foreach (var method in
                type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                foreach (var attribute in method.GetCustomAttributes(true))
                {
                    if (attribute is PropertyListenerAttribute propertyListenerAttribute)
                    {
                        methods.Add(new PropertyListenerInfo(method, propertyListenerAttribute.NameFilter));
                    }
                }
            }

            PropertyListeners[type] = methods;

            return methods;
        }
    }
}
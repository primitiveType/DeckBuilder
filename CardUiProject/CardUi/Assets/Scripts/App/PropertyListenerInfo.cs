using System.Reflection;

namespace App
{
    public struct PropertyListenerInfo
    {
        public PropertyListenerInfo(MethodInfo methodInfo, string filter)
        {
            MethodInfo = methodInfo;
            Filter = filter;
        }

        public MethodInfo MethodInfo { get; }
        public string Filter { get; }
    }
}
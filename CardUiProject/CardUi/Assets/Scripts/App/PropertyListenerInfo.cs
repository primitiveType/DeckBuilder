using System.Reflection;

namespace Common
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
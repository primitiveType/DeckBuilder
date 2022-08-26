using System;
using Api;

namespace SummerJam1Tests
{
    public class TestLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}

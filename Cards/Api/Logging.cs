using System;

namespace Api
{
    public class Logging : ILogger
    {
        private static ILogger s_logger;

        public static ILogger s_Logger
        {
            get
            {
                if (s_logger == null)
                {
                    throw new NullReferenceException(
                        $"Logger was not initialized. You must call {nameof(Api)}.{nameof(Logging)}.{nameof(Initialize)} to set up the logger.");
                }

                return s_logger;
            }
            set => s_logger = value;
        }

        void ILogger.LogWarning(string message)
        {
            s_Logger.LogWarning(message);
        }

        void ILogger.LogError(string message)
        {
            s_Logger.LogError(message);
        }

        void ILogger.Log(string message)
        {
            s_Logger.Log(message);
        }

        public static void Initialize(ILogger logger)
        {
            s_Logger = logger;
            s_Logger.Log("Initialized Logger instance.");
        }

        public static void Log(string message)
        {
            s_Logger.Log(message);
        }

        public static void LogWarning(string message)
        {
            s_Logger.LogWarning(message);
        }

        public static void LogError(string message)
        {
            s_Logger.LogError(message);
        }
    }
}
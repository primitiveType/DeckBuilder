namespace Api
{
    public class Logging : ILogger
    {
        public static ILogger s_Logger { get; set; }

        public static void Initialize(ILogger logger)
        {
            s_Logger = logger;
        }
    }
}

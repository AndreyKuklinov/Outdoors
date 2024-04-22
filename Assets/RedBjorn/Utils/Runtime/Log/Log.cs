namespace RedBjorn.Utils
{
    public class Log<T> where T : ILogger, new()
    {
        static T LoggerCached;
        static T Logger
        {
            get
            {
                Init(true, true, true);
                return LoggerCached;
            }
        }

        public static void Init(bool infoEnabled, bool warningEnabled, bool errorEnabled)
        {
            if (LoggerCached == null)
            {
                LoggerCached = new T();
                LoggerCached.Init(string.Concat("[", typeof(T).Name, "] "), infoEnabled, warningEnabled, errorEnabled);
            }
        }

        public static void I(object message)
        {
            Logger.Info(message);
        }

        public static void W(object message)
        {
            Logger.Warning(message);
        }

        public static void E(object message)
        {
            Logger.Error(message);
        }
    }
}


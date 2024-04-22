using UnityEngine;

namespace RedBjorn.Utils
{
    /// <summary>
    /// RedBjorn.Utils.ILogger implementation which wraps UnityEngine.Debug.Log with customizable enable/disable logic
    /// </summary>
    public abstract class Logger : RedBjorn.Utils.ILogger
    {
        string Prefix;
        bool InfoEnabled;
        bool WarningEnabled;
        bool ErrorEnabled;

        public void Init(string prefix, bool infoEnabled, bool warningEnabled, bool errorEnabled)
        {
            Prefix = prefix;
            InfoEnabled = infoEnabled;
            WarningEnabled = warningEnabled;
            ErrorEnabled = errorEnabled;
        }

        public void Error(object message)
        {
            if (ErrorEnabled)
            {
                Debug.LogError(Prefix + message);
            }
        }

        public void Info(object message)
        {
            if (InfoEnabled)
            {
                Debug.Log(Prefix + message);
            }
        }

        public void Warning(object message)
        {
            if (WarningEnabled)
            {
                Debug.LogWarning(Prefix + message);
            }
        }
    }
}

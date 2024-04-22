namespace RedBjorn.Utils
{
    public interface ILogger
    {
        void Init(string prefix, bool infoEnabled, bool warningEnabled, bool errorEnabled);
        void Info(object message);
        void Warning(object message);
        void Error(object message);
    }
}

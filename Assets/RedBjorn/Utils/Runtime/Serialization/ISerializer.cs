namespace RedBjorn.Utils
{
    public interface ISerializer
    {
        void Serialize(object data, string name);
        void Serialize(object data, string name, System.Action<bool, string, byte[]> onSerialized);
        void Deserialize<T>(byte[] data, string name);
        void Deserialize<T>(byte[] data, string name, System.Action<bool, string, T> onDeserialized);
    }
}

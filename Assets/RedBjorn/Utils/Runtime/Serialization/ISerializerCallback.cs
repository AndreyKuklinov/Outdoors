namespace RedBjorn.Utils
{
    public interface ISerializerCallback
    {
        void OnSerializeCompleted(bool success, string name, byte[] serialized);
        void OnDeserializeCompleted(bool success, string name, object deserialized);
    }
}

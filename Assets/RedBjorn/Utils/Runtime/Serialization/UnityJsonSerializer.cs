using System.Text;
using UnityEngine;

namespace RedBjorn.Utils
{
    public class UnityJsonSerializer : ISerializer
    {
        ISerializerCallback Callback;

        public UnityJsonSerializer() { }

        public UnityJsonSerializer(ISerializerCallback callback)
        {
            Callback = callback;
        }

        public void Serialize(object data, string name)
        {
            var result = true;
            var dataString = JsonUtility.ToJson(data, true);
            var serialized = Encoding.UTF8.GetBytes(dataString);
            if (Callback != null)
            {
                Callback.OnSerializeCompleted(result, name, serialized);
            }
        }

        public void Serialize(object data, string name, System.Action<bool, string, byte[]> onSerialized)
        {
            var result = true;
            var dataString = JsonUtility.ToJson(data, true);
            var serialized = Encoding.UTF8.GetBytes(dataString);
            onSerialized?.Invoke(result, name, serialized);
        }

        public void Deserialize<T>(byte[] data, string name)
        {
            try
            {
                var dataString = Encoding.UTF8.GetString(data);
                var deserialized = JsonUtility.FromJson<T>(dataString);
                if (Callback != null)
                {
                    Callback.OnDeserializeCompleted(true, name, deserialized);
                }
            }
            catch (System.Exception e)
            {
                Log.E($"Deserialization failed {name} due to {e.Message}");
                if (Callback != null)
                {
                    Callback.OnDeserializeCompleted(false, name, default(T));
                }
            }
        }

        public void Deserialize<T>(byte[] data, string name, System.Action<bool, string, T> onDeserialized)
        {
            try
            {
                var dataString = Encoding.UTF8.GetString(data);
                var deserialized = JsonUtility.FromJson<T>(dataString);
                onDeserialized?.Invoke(true, name, deserialized);
            }
            catch (System.Exception e)
            {
                Log.E($"Deserialization failed {name} due to {e.Message}");
                onDeserialized?.Invoke(false, name, default(T));
            }
        }
    }
}

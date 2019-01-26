using System.Text;
using Newtonsoft.Json;

namespace SerialiserBenchmarks.Serialisers
{
    public class NewtonsoftJson : ISerialiser
    {
        public byte[] Serialise<T>(T instance)
        {
            string json = JsonConvert.SerializeObject(instance);
            return Encoding.UTF8.GetBytes(json);
        }

        public T Deserialise<T>(byte[] bytes)
        {
            string json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
using Newtonsoft.Json;

namespace Api
{
    public class Serializer
    {
        public static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        public static string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o, Settings);
        }

        public static T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str, Settings);
        }
    }
}
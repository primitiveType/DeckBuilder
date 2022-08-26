using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace Api
{
    public class Serializer
    {
        public static JsonSerializerSettings Settings = new()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ContractResolver = new PrivateSetterContractResolver()
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
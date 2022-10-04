using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace Api
{
    public static class Serializer
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ContractResolver = new PrivateSetterContractResolver(), 
            DefaultValueHandling = DefaultValueHandling.Ignore,
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

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
            ContractResolver = new IgnoreNoSetContractResolver(),
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Converters = { new DefaultToUnknownConverter(), new ComponentConverter() },
            Formatting = Formatting.Indented
        };

        private static readonly JsonSerializerSettings NoIdSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ContractResolver = new NoIdContractResolver(),
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Converters = { new DefaultToUnknownConverter(), new ComponentConverter() },
            Formatting = Formatting.Indented
        };

        public static string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o, Settings);
        }
        
        public static string SerializeWithoutIds(object o)
        {
            return JsonConvert.SerializeObject(o, NoIdSettings);
        }

        public static T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str, Settings);
        }
    }
}
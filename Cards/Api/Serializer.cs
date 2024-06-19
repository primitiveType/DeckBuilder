using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Converters = { new DefaultToUnknownConverter(), new ComponentConverter() },
            Formatting = Formatting.Indented
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

    public class ComponentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IChildrenCollection<Component> ).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Console.WriteLine("Writing !");
            serializer.Serialize(writer, ((IChildrenCollection<Component>)value).Where(ShouldSerialize).ToArray());
        }

        private bool ShouldSerialize(Component arg)
        {
            return !arg.GetType().GetCustomAttributes<NonSerializableComponentAttribute>().Any();
        }
    }

    public class DefaultToUnknownConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Component).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jObject = JObject.Load(reader);
            try
            {
                // attempt to deserialize to known type
                using (JsonReader jObjectReader = CopyReaderForObject(reader, jObject))
                {
                    // create new serializer, as opposed to using the serializer parm, to avoid infinite recursion
                    JsonSerializer tempSerializer = new JsonSerializer()
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    };
                    return tempSerializer.Deserialize(jObjectReader);
                }
            }
            catch (JsonSerializationException)
            {
                // default to Unknown type when deserialization fails
                return jObject.ToObject<UnknownComponent>();
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public static JsonReader CopyReaderForObject(JsonReader reader, JToken jToken)
        {
            // create reader and copy over settings
            JsonReader jTokenReader = jToken.CreateReader();
            jTokenReader.Culture = reader.Culture;
            jTokenReader.DateFormatString = reader.DateFormatString;
            jTokenReader.DateParseHandling = reader.DateParseHandling;
            jTokenReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jTokenReader.FloatParseHandling = reader.FloatParseHandling;
            jTokenReader.MaxDepth = reader.MaxDepth;
            jTokenReader.SupportMultipleContent = reader.SupportMultipleContent;
            return jTokenReader;
        }
    }
}
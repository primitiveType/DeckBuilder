using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api
{
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
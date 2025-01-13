using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Api
{
    public class ComponentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IChildrenCollection<Component>).IsAssignableFrom(objectType);
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
            serializer.Serialize(writer, ((IChildrenCollection<Component>)value).Where(ShouldSerialize).ToArray());
        }

        private bool ShouldSerialize(Component arg)
        {
            return !arg.GetType().GetCustomAttributes<NonSerializableComponentAttribute>().Any();
        }
    }
}
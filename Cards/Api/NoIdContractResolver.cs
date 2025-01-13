using System.Reflection;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api
{
    public class NoIdContractResolver : PrivateSetterContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.PropertyName == nameof(Entity.Id))
            {
                property.ShouldSerialize = (_) => false;
            }

            return property;
        }
    }
}
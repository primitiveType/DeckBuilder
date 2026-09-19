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
            if (member.MemberType == MemberTypes.Property)
            {
                if (((PropertyInfo)member).SetMethod == null)
                {
                    property.ShouldSerialize = (_) => false;
                }    
            }
            return property;
        }
    }
    
    public class IgnoreNoSetContractResolver : PrivateSetterContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (member.MemberType == MemberTypes.Property)
            {
                if (((PropertyInfo)member).SetMethod == null)
                {
                    property.ShouldSerialize = (_) => false;
                }    
            }

            return property;
        }
    }
}
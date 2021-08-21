using System;
using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DeckbuilderLibrary.Data
{
    internal class GameEntityConverter : JsonConverter<IGameEntity>
    {
        public override bool CanRead { get; } = true;
        public override bool CanWrite { get; } = false;


        public override void WriteJson(JsonWriter writer, IGameEntity value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IGameEntity ReadJson(JsonReader reader, Type objectType, IGameEntity existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (!token.HasValues)
                throw new InvalidOperationException("invalid object");

            var typeToken = token["$type"];
            if (typeToken == null)
                throw new InvalidOperationException("invalid object");
            Type actualType = Type.GetType(typeToken.Value<string>());
            if (existingValue == null || existingValue.GetType() != actualType)
            {
                var contract = serializer.ContractResolver.ResolveContract(actualType);
                existingValue = (IGameEntity)contract.DefaultCreator();
            }

            using (var subReader = token.CreateReader())
            {
                // Using "populate" avoids infinite recursion.
                serializer.Populate(subReader, existingValue);
            }

            IInternalGameEntity internalGameEntity = ((IInternalGameEntity)existingValue);
            internalGameEntity.SetContext(GameContext.CurrentContext);
            internalGameEntity.InternalInitialize();
            existingValue.Context.AddEntity(existingValue);
            return existingValue;
            // var newEntity = (IGameEntity) Activator.CreateInstance(objectType);
            // serializer.Populate(reader, newEntity);
            //

            // return newEntity;
        }
    }
}
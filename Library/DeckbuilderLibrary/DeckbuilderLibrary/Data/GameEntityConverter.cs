using System;
using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeckbuilderLibrary.Data
{
    internal class GameEntityConverter : JsonConverter<GameEntity>
    {
        public override bool CanRead { get; } = true;
        public override bool CanWrite { get; } = false;


        public override void WriteJson(JsonWriter writer, GameEntity value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override GameEntity ReadJson(JsonReader reader, Type objectType, GameEntity existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var typeToken = token["$type"];
            if (typeToken == null)
                throw new InvalidOperationException("invalid object");
            Type actualType = Type.GetType(typeToken.Value<string>());
            if (existingValue == null || existingValue.GetType() != actualType)
            {
                var contract = serializer.ContractResolver.ResolveContract(actualType);
                existingValue = (GameEntity) contract.DefaultCreator();
            }
            using (var subReader = token.CreateReader())
            {
                // Using "populate" avoids infinite recursion.
                serializer.Populate(subReader, existingValue);
            }
            existingValue.Context = GameContext.CurrentContext;
            existingValue.Initialize();
            existingValue.Context.AddEntity(existingValue);
            return existingValue;
            // var newEntity = (GameEntity) Activator.CreateInstance(objectType);
            // serializer.Populate(reader, newEntity);
            //
        
            // return newEntity;
        }
    }
}
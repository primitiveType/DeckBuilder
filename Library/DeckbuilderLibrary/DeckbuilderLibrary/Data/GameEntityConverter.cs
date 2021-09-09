﻿using System;
using DeckbuilderLibrary.Data.GameEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeckbuilderLibrary.Data
{
    internal class GameEntityCloneConverter : GameEntityConverter
    {
        protected override bool IsCloning => true;
    }

    internal class GameEntityConverter : JsonConverter<IGameEntity>
    {
        public override bool CanRead { get; } = true;
        public override bool CanWrite { get; } = false;
        protected virtual bool IsCloning => false;

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
            {
                return null;
                // throw new InvalidOperationException("invalid object");
            }

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
            if (IsCloning)
            {
                ((GameEntity)internalGameEntity).Id = -1;
            }
            ((IInternalGameContext)existingValue.Context).ToInitializeAdd(internalGameEntity);
            return existingValue;
           
        }
    }
}
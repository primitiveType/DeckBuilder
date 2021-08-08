using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Card : GameEntity
    {
        private Script Script { get; set; }

        public string Name { get; set; }
        
        //TODO: this property should not be necessary. The save data should only ever exist when serializing/deserializing.
        //We only need it right now because we don't have a good way to get the current api while creating this object.
        public double? SaveData { get; set; }

        [JsonConstructor]
        public Card(string name, double? saveData)
        {
            Name = name;
            SaveData = saveData;
            Debug.Log("Default ctor.");
        }

        public void InitializeScript(Script script)
        {
            Script = script;
            Script.Call(Script.Globals["cardInstanceCreated"], Id, SaveData);
        }

  

        [OnSerializing]
        private void OnSerializing(StreamingContext streamingContext)
        {
            DynValue data = Script.Call(Script.Globals["getCardData"], Id);
            var number = data.CastToNumber();
            SaveData = number;
        }
        
        [OnSerialized]
        private void OnSerialized(StreamingContext streamingContext)
        {
            SaveData = null;
        }

        public List<Actor> GetValidTargets()
        {
            List<Actor> actors = new List<Actor>();
            DynValue values = Script.Call(Script.Globals["getValidTargets"], Id);
            foreach (DynValue value in values.Table.Values)
            {
                double? number = value.CastToNumber();
                int id = number.HasValue ? (int) number.Value : -1;
                actors.Add(Api.GetActorById(id));
            }

            return actors;
        }

        public void PlayCard(Actor target)
        {
            Script.Call(Script.Globals["playCard"], Id, target.Id);
            Script.Call(Script.Globals["onCardPlayed"], Id);
        }


        private void Log(string log)
        {
            Script.Call(Script.Globals["log"], Id, log);
        }

        public Card Duplicate()
        {
            DynValue data = Script.Call(Script.Globals["getCardData"], Id);
            Card copy = Api.CreateCardInstance(Name, data);
            return copy;
        }
    }
}
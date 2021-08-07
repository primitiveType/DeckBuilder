using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Data
{
    public class Card : GameEntity
    {
        private Script Script { get; }
        private GlobalApi Api { get; }

        public string Name { get; set; }

        public Card(Script script, string name, GlobalApi api, DynValue opaqueData) //TODO: this shouldn't need the current api.
        {
            Script = script;
            Api = api;
            Name = name;
            Script.Call(Script.Globals["cardInstanceCreated"], Id, opaqueData);
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
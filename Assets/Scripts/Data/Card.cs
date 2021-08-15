using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Card : GameEntity
    {
        private Script Script { get; set; }

        public string Name { get; set; }
        

        public Card(string name,  IContext context) : base(context)
        {
            Name = name;
            SetupEvents();
            Script = Context.GetCardScript(Name);
        }

        public Card(Card card) : base(card.Context)
        {
            Name = card.Name;
            InitializeScript(card.Script);//assumes same context. a true copy ctor.
            Properties = card.Properties;
            SetupEvents();
        }

        private void SetupEvents()
        {
            Context.Events.DamageDealt -= GameEventsOnDamageDealt;
            Context.Events.CardPlayed -= GameEventsOnCardPlayed;
            Context.Events.CardCreated -= GameEventsOnCardCreated;
            Context.Events.CardMoved -= GameEventsOnCardMoved;
            
            Context.Events.DamageDealt += GameEventsOnDamageDealt;
            Context.Events.CardPlayed += GameEventsOnCardPlayed;
            Context.Events.CardCreated += GameEventsOnCardCreated;
            Context.Events.CardMoved += GameEventsOnCardMoved;
        }

        [JsonConstructor]
        private Card(int id, Properties properties, string name) : base(id, properties)
        {
            Name = name;
            SetupEvents();
            Script = Context.GetCardScript(Name);
        }

        private void GameEventsOnCardMoved(object sender, CardMovedEventArgs args)
        {
            Script.Call(Script.Globals["onCardMoved"], args.MovedCard);//TODO: pile info needs to go to lua.
        }

        private void GameEventsOnCardCreated(object sender, CardCreatedEventArgs args)
        {
            Script.Call(Script.Globals["onCardCreated"], args.CardId);
        }

        private void GameEventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Script.Call(Script.Globals["onThisCardPlayed"], args.CardId);
            }
          
            Script.Call(Script.Globals["onAnyCardPlayed"], args.CardId);
        }

        private void GameEventsOnDamageDealt(object sender, DamageDealtArgs args)
        {
            Script?.Call(Script?.Globals["onDamageDealt"], args.ActorId, args.TotalDamage, args.HealthDamage);
        }

        public void InitializeScript(Script script)
        {
            Script = script;
            Script.Call(Script.Globals["cardInstanceCreate"], Id);
        }


        public List<Actor> GetValidTargets()
        {
            List<Actor> actors = new List<Actor>();
            DynValue values = Script.Call(Script.Globals["getValidTargets"], Id);
            foreach (DynValue value in values.Table.Values)
            {
                double? number = value.CastToNumber();
                int id = number.HasValue ? (int) number.Value : -1;
                actors.Add(Context.GetActorById(id));
            }

            return actors;
        }

        public void PlayCard(Actor target)
        {
            Script.Call(Script.Globals["playCard"], Id, target.Id);
            ((IGameEventHandler) Context.Events).InvokeCardPlayed(this, new CardPlayedEventArgs(Id));
        }


        private void Log(string log)
        {
            Script.Call(Script.Globals["log"], Id, log);
        }
        
        
        
    }
}
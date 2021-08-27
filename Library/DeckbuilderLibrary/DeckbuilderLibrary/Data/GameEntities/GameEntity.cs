using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeckbuilderLibrary.Data.Events;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public abstract class GameEntity : IInternalGameEntity
    {
        [JsonProperty] public int Id { get; internal set; } = -1;

        [JsonIgnore] public IContext Context { get; private set; }

        [JsonIgnore] private bool Initialized { get; set; }

        private List<Action> TerminationActions = new List<Action>();

        void IInternalGameEntity.InternalInitialize()
        {
            if (Initialized)
            {
                throw new NotSupportedException(
                    $"Attempted to initialize an already initialized Entity with id {Id}. This is not supported.");
            }

            Initialize();
            SetupEvents();
            Initialized = true;
        }

        private void SetupEvents()
        {
            MethodInfo[] methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                            BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var method in methodInfos)
            {
                var attribute = method.GetCustomAttribute<BattleEventAttribute>(true);
                if (attribute != null)
                {
                    var d = Delegate.CreateDelegate(typeof(CardPlayedEvent), this, method);
                    var eventInfo = Context.Events.GetType().GetEvent(nameof(Context.Events.CardPlayed));
                    var addMethod = eventInfo.GetAddMethod();
                    Object[] addHandlerArgs = { d };
                    addMethod.Invoke(Context.Events, addHandlerArgs);
                    
                    // var removeMethod = eventInfo.GetRemoveMethod();
                    // removeMethod.Invoke(Context.Events, addHandlerArgs);
                   
                    // Context.Events.CardPlayed += (o, args) => { method.Invoke(this, new[] { o, args }); };
                    // TerminationActions.Add(() => Conte);
                }

                var battleEntityAttribute = method.GetCustomAttribute<BattleEntityAttribute>(true);
                if (battleEntityAttribute != null)
                {
                    Context.Events.BattleEnded += (o, args) => { this.Terminate(); };
                }
            }
        }

        private void Terminate()
        {
        }

        public void SetContext(IContext context)
        {
            if (Context != null)
            {
                throw new NotSupportedException(
                    $"Attempted to set context on an already initialized Entity with id {Id}. This is not supported.");
            }

            Context = context;
        }

        internal GameEntity()
        {
        }


        protected virtual void Initialize()
        {
        }

        // public Properties Properties { get; protected set; } = new Properties();
    }
}
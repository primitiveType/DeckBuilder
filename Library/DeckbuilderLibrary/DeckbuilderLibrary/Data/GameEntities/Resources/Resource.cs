using System;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public interface IResource<T> : IArithmetic<T> where T : IResource<T>
    {
    }

    public abstract class Resource<T> : GameEntity, IArithmetic<T>, IInternalResource where T : Resource<T>
    {
        private IActor m_Owner;
        public abstract string Name { get; }

        [JsonIgnore]
        public IActor Owner
        {
            get => m_Owner;
            set
            {
                m_Owner = value;
                if (Owner == null)
                {
                    OwnerId = -1;
                }
                else
                {
                    OwnerId = Owner.Id;
                }
            }
        }

        [JsonProperty] public int OwnerId { get; set; } = -1;
        public virtual int Amount { get; protected set; }

        int IInternalResource.Amount
        {
            get => Amount;
            set => Amount = value;
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (Owner == null)
            {
                if (OwnerId == -1)
                {
                    throw new ArgumentException("Tried to initialize resource with no Owner info!");
                }

                Owner = Context.GetActorById(OwnerId);
            }
            else
            {
                OwnerId = Owner.Id;
            }
        }

        public virtual void Add(int amount)
        {
            Amount += amount;
        }

        public virtual void Subtract(int amount)
        {
            Amount -= amount;
            if (Amount < 0)
            {
                Amount = 0;
            }
        }

        public virtual void Set(int amount)
        {
            Amount = amount;
        }


        public void Add(T amount)
        {
            Amount = amount.Amount;
        }

        public void Subtract(T amount)
        {
            Amount -= amount.Amount;
        }
    }
}
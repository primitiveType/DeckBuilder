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
        private int m_Amount;
        public abstract string Name { get; }
        private EntityReference<IGameEntity> OwnerEntityReference { get; set; } = new EntityReference<IGameEntity>();

        [JsonIgnore]
        public IGameEntity Owner
        {
            get => OwnerEntityReference.Entity ;
            set => OwnerEntityReference.Entity = value;
        }


        public virtual int Amount
        {
            get => m_Amount;
            protected set
            {
                m_Amount = value;
                if (m_Amount < 0)
                    m_Amount = 0;
            }
        } //TODO: I think an event should be invoked in here.

        int IInternalResource.Amount
        {
            get => Amount;
            set => Amount = value;
        }

        protected override void Initialize()
        {
            base.Initialize();
            // if (Owner == null)
            // {
            //     if (OwnerId == -1)
            //     {
            //         throw new ArgumentException("Tried to initialize resource with no Owner info!");
            //     }
            //
            //     Owner = Context.GetCurrentBattle().GetActorById(OwnerId);
            // }
            // else
            // {
            //     OwnerId = Owner.Id;
            // }
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
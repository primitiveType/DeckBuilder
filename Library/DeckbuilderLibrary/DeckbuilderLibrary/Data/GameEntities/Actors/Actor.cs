using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using DeckbuilderLibrary.Extensions;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public abstract class Actor : GameEntity, IInternalActor
    {
        private CubicHexCoord m_Coordinate;
        public int Health => Resources.GetResourceAmount<Health>();

        public int Armor => Resources.GetResourceAmount<Armor>();

        [JsonProperty] public Resources.Resources Resources { get; private set; }
        [JsonIgnore] public ActorNode Node => Coordinate.ToActorNode(Context);

        public CubicHexCoord Coordinate
        {
            get => m_Coordinate;
            private set => SetField(ref m_Coordinate, value);
        }

        CubicHexCoord ICoordinateProperty.Coordinate => Coordinate;

        CubicHexCoord IInternalCoordinateProperty.Coordinate
        {
            get => Coordinate;
            set => Coordinate = value;
        }


        protected override void Initialize()
        {
            base.Initialize();
            if (Resources == null)
            {
                Resources = Context.CreateEntity<Resources.Resources>();
            }

            Resources.Owner = this;
            Context.Events.ActorDied += OnActorDied;
        }

        private void OnActorDied(object sender, ActorDiedEventArgs args)
        {
            if (args.Actor != this)
            {
                return;
            }

            if (Context.GetCurrentBattle().Graph.TryGetNode(Coordinate, out ActorNode node))
            {
                node.TryRemove(this);
            }
        }

        void IInternalActor.TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage)
        {
            //this method should instead go to the vitality component
            int armorDamage = System.Math.Min(damage, Armor);
            Resources.SubtractResource<Armor>(armorDamage);
            healthDamage = damage - armorDamage;
            healthDamage = System.Math.Min(healthDamage, Health);
            totalDamage = healthDamage + armorDamage;

            Resources.SubtractResource<Health>(healthDamage);

            //do we want to clamp the output of damage dealt? add overkill damage as a param?
            ((IInternalBattleEventHandler)Context.Events).InvokeDamageDealt(this,
                new DamageDealtArgs(Id, totalDamage, healthDamage, source));

            if (Health <= 0)
            {
                ((IInternalBattleEventHandler)Context.Events).InvokeActorDied(this,
                    new ActorDiedEventArgs(this, source, source /*TODO*/));
            }
        }
    }
}
using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data.Events
{
    public class RequestDamageAmountEventArgs
    {
        public IGameEntity Target { get; }
        private List<DamageAmountModifier> Modifiers { get; } = new List<DamageAmountModifier>();

        public IGameEntity Owner { get; }

        public void AddModifier(DamageAmountModifier mod)
        {
            Modifiers.Add(mod);
        }

        public RequestDamageAmountEventArgs( IGameEntity owner, IGameEntity target)
        {
            Target = target;
            Owner = owner;
        }

        public int GetResult()
        {
            int total = 0;
            foreach (var mod in Modifiers)
            {
                total += mod.AdditiveModifier;
            }

            float totalPercentMod = 1;
            foreach (var mod in Modifiers)
            {
                totalPercentMod += mod.MultiplicativeModifier;
            }

            return (int)(total * totalPercentMod);
        }
    }
}
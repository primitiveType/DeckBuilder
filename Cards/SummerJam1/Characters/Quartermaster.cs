using Api;
using SummerJam1.Cards;
using SummerJam1.Relics;

namespace SummerJam1.Characters
{
    public class Quartermaster : SummerJam1Component, ICharacterClass
    {
        public string Name => "Quartermaster";
        protected override void Initialize()
        {
            base.Initialize();
            //starting relic.
            Entity.AddComponent<BagOfHolding>();
            //set up starting cards.
            PopulatePlayerDeck();
        }
        private void PopulatePlayerDeck()
        {
            foreach (StartingCard starterCard in Game.PrefabsContainer.GetComponentsInChildren<StartingCard>())
            {
                var constraint = starterCard.Entity.GetComponent<CharacterConstraint>();
                if (constraint is { Character: not ("Any" or "Quartermaster") }) continue;

                for (int i = 0; i < starterCard.Amount; i++)
                {
                    Context.CreateEntity(Game.Deck.Entity,
                        starterCard.Entity.GetComponent<SourcePrefab>().Prefab);
                }
            }
        }

    }
}
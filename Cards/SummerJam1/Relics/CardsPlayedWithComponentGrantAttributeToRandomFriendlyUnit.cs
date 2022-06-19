using System.Collections.Generic;
using Api;
using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1.Relics
{
    public class CardsPlayedWithComponentGrantAttributeToRandomFriendlyUnit<TComponent, TAttribute> : SummerJam1Component
        where TComponent : Component where TAttribute : Component, IAmount, new()
    {
        [JsonProperty] public virtual int Amount { get; set; } = 1;


        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId.GetComponent<TComponent>() != null)
            {
                List<IEntity> friendlies = Game.Battle.GetFriendlies();
                if (friendlies.Count == 0)
                {
                    return;
                }

                var index = Game.Random.SystemRandom.Next(0, friendlies.Count);

                friendlies[index].GetOrAddComponent<TAttribute>().Amount += Amount;
            }
        }
    }
}

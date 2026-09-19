using Api;
using SummerJam1.Cards;

namespace SummerJam1
{
    public static class GameExtensions
    {
        //When a player plays a card, the card deals the damage. 
        //but sometimes we want some effects to refer to the player, not the card.
        public static IEntity GetOwner(this IEntity entity)
        {
            var card = entity.GetComponent<PlayerCard>();
            if (card != null)
            {
                return entity.Context.Root.GetComponent<Game>().Player.Entity;
            }

            return entity;
        }
    }
}
using Api;

namespace CardsAndPiles
{
    public class CardPrefabPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}

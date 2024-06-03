using Api;

namespace CardsAndPiles.Components
{
    public class DoNothingCard : Card
    {
        protected override bool PlayCard(IEntity target)
        {
            return true;
        }
    }
}

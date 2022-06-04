using System.ComponentModel;
using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class Unit : Api.Component, IPileItem
    {
        public bool TrySendToPile(IPile pile)
        {
            return pile is UnitSlot;
        }
    }

    public class UnitCard : Card, IPileItem
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Parent.Id)
            {
                var unit = new Entity();
                Parent.GetComponentInParent<SummerJam1Events>().OnUnitCreated();
            }
        }
    }
}
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class AddsComponentOnHit<T> : SummerJam1Component, IAmount, IDescription where T : Component, IAmount, new()
    {
        public int Amount { get; set; } = 1;
        

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                var status = args.Target.AddComponent<T>();
                status.Amount = Amount;
            }
        }

        public string Description => $"Apply {Amount} {typeof(T).Name}";
    }
}
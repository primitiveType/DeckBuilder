namespace SummerJam1.Units
{
    public class CardPrize : SummerJam1Component
    {
        public string Prefab { get; set; }

        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                Game.PrizePile.AddPrefab(Prefab);
            }
        }
    }
}
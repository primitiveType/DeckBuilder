namespace SummerJam1
{
    public class BattleChoice : EncounterChoice
    {
        public override string Description { get; } = "A battle, with a booster pack as reward.";

        public override void Click()
        {
            Game.StartBattle();
        }
    }
}
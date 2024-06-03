namespace SummerJam1
{
    public class RelicEncounter : Encounter
    {
        public string Prefab { get; set; }

        protected override void PlayerEnteredCell()
        {
            Context.CreateEntity(Game.RelicPrizePile.Entity, Prefab);
            Entity.Destroy();
        }
    }
}
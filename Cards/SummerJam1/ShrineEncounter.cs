namespace SummerJam1
{
    public class ShrineEncounter : Encounter
    {
        protected override void PlayerEnteredCell()
        {
            Events.OnRequestRemoveCard(new RequestRemoveCardEventArgs());
            Entity.Destroy();
        }
    }
}
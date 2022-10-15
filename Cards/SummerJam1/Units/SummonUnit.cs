using System.Linq;
using Api;
using CardsAndPiles;

namespace SummerJam1.Units
{
    public class SummonUnit : SummerJam1Component
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            var reference = Entity.GetComponent<PrefabReference>();
            IEntity prefab = Context.CreateEntity(null, reference.Prefab);
            RequestPlayCardEventArgs request = new(prefab.Parent, args.Target);//request to play the card we are creating...
            Events.OnRequestPlayCard(request);
            //don't actually play it, just see if its playable.

            if (request.Blockers.Any())
            {
                args.Blockers.AddRange(request.Blockers);
            }
            
            prefab.Destroy();//cleanup our fake tester instance of the card...
        }
       
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            PrefabReference reference = Entity.GetComponent<PrefabReference>();
            // var prefab = Game.PrefabsContainer.GetComponentsInChildren<SourcePrefab>().FirstOrDefault(source => source.Prefab == reference.Prefab);
            IEntity prefab = Context.CreateEntity(null, reference.Prefab);
            
            //theoretically unnecessary since we did it during our own request above...?
            RequestPlayCardEventArgs request = new(prefab.Parent, args.Target);//request to play the card we are creating...
            Events.OnRequestPlayCard(request);
         
            
            if (request.Blockers.Any())
            {
                Logging.LogWarning("Failed to summon card. This shouldn't be possible.");
                prefab.Destroy();
            }
            else
            {
                Events.OnCardPlayed(new CardPlayedEventArgs(prefab, args.Target, true));
            }
            
            
        }
    }
}
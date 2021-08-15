using UnityEngine;

public class PileView : View<Pile>
{//I think this thing would probably only care about how many cards are in it... and might have inheritors for different
    //types of piles. Should it also initialize card proxies?
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized pile proxy with id {GameEntity.Id}.");
        GameEntity.Context.Events.CardCreated += GameEventHandlerOnCardCreated;
        GameEntity.Context.Events.CardMoved += GameEventHandlerOnCardMoved;
    }

    private void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        //not sure pile type needs to exist. maybe that enum can just go away... not sure.
        if (args.NewPileType == GameEntity.PileType && args.PreviousPileType != GameEntity.PileType)
        {//this pile is receiving a card, so we create a proxy for it. That way different piles can display cards differently.
            CreateCardProxy(args.MovedCard);
        }
        else if (args.NewPileType != GameEntity.PileType && args.PreviousPileType == GameEntity.PileType)
        {
            //destroy the card that's being moved away. the other pile should create a new proxy.
            //Not sure how this will work with timing/animations and such.
            DestroyCardProxy(args.MovedCard);
        }
    }

    private void DestroyCardProxy(int argsMovedCard)
    {
    }

    private void CreateCardProxy(int argsMovedCard)
    {
        //instantiate
        //initialize
    }

    private void GameEventHandlerOnCardCreated(object sender, CardCreatedEventArgs args)
    {
    }
}
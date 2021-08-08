using System.Collections.Generic;
using Data;

public class Battle : GameEntity
{
    public Actor Player { get; }
    public List<Actor> Enemies { get; }
    public Deck Deck { get; }

    public Battle(Actor player, List<Actor> enemies, Deck deck)
    {
        Player = player;
        Enemies = enemies;
        Deck = deck;
        GameEvents.CardMoved += DeckOnCardMoved;
    }

    private void DeckOnCardMoved(object sender, CardMovedEventArgs args)
    {
        //should battle mediate events so that the ui, etc have one place to listen to?
    }
}

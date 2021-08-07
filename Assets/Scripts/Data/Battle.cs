using System.Collections.Generic;
using Data;

public class Battle
{
    public Actor Player { get; }
    public List<Actor> Enemies { get; }
    public Deck Deck { get; }

    public Battle(Actor player, List<Actor> enemies, Deck deck)
    {
        Player = player;
        Enemies = enemies;
        Deck = deck;
    }
}

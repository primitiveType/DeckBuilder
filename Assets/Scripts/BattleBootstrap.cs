using System.Collections.Generic;
using Data;
using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        Actor player = new Actor(100);
        Actor enemy = new Actor(100);
        
        Battle battle = new Battle(player, new List<Actor>{enemy}, new Deck());
        
        // GlobalApi.SetCurrentBattle(battle);
        
        //At this point we have basic objects representing the player and an enemy.
        //There's an api holding the battle that should be able to provide access to those objects from LUA.
        //We'll probably need to instantiate the player's deck each battle- basically creating a copy of it,
        //since there are, for example, cards that exhaust (destroyed during battle) that come back afterwards.
        //So the deck at start of battle should sort of be a snapshot...
        //But there could also be things like a card that gets permanently stronger every time you use it- 
        //that would have to affect the actual out-of-battle instance of the card. How do we handle that?
        //Is it a flyweight? or maybe a prototype?
        //how would we handle a card that duplicates other cards in battle?

        //layers as I see them:
        //A card prototype. Every instance of a card comes from one of these. It defines what the card does. every prototype has one lua script.
        //A deck-instance of a card. When a player outside of battle adds a new card to the deck, one of these is created from the above prototype.
        //A battle-instance of a card. When a battle starts, your battle deck is populated with these. That way cards can exhaust without you permanently losing them.

        //A card that creates a dup in-battle would basically just draft a fresh prototype (probably) and add it to the battle deck. The battle deck is disposed of after battle.
        //We would need a mechanism for deck-instances of cards to still listen to what happens in battle. Might just be a separate set of callbacks on the lua side?
        //each layer would need to be able to know about the one before it in a sense. a battle-instance card should know what its prototype is for instance.
        //Can i duplicate a lua script object? Will it keep any state? If not, can we have the lua scripts track state and create cards into the deck?
        //Should lua have an opaque that it can provide when duplicated?
    }
    
}
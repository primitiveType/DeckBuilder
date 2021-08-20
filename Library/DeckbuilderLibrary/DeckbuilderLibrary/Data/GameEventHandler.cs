using System.Collections.Generic;
using Data;

public class GameEventHandler : IInternalGameEventHandler
{
    public event CardMovedEvent CardMoved;

    public event RequestDamageAmountEvent RequestDamageAmount;

    public void InvokeCardMoved(object sender, CardMovedEventArgs args)
    {
        CardMoved?.Invoke(sender, args);
    }

    public event CardPlayedEvent CardPlayed;

    public void InvokeCardPlayed(object sender, CardPlayedEventArgs args)
    {
        CardPlayed?.Invoke(sender, args);
    }

    public event CardCreatedEvent CardCreated;

    public void InvokeCardCreated(object sender, CardCreatedEventArgs args)
    {
        CardCreated?.Invoke(sender, args);
    }

    public event DamageDealt DamageDealt;

    public void InvokeDamageDealt(object sender, DamageDealtArgs args)
    {
        DamageDealt?.Invoke(sender, args);
    }

    public int RequestDamage(object sender, int baseDamage, IGameEntity target)
    {
        var args = new RequestDamageAmountEventArgs(target);
        args.AddModifier(new DamageAmountModifier { AdditiveModifier = baseDamage });
        RequestDamageAmount?.Invoke(sender, args);
        return args.GetResult();
    }
}

class GameEventHandlerImpl : GameEventHandler
{
}

public delegate void RequestDamageAmountEvent(object sender, RequestDamageAmountEventArgs args);

public class RequestDamageAmountEventArgs
{
    public IGameEntity Target { get; }
    private List<DamageAmountModifier> Modifiers { get; } = new List<DamageAmountModifier>();


    public void AddModifier(DamageAmountModifier mod)
    {
        Modifiers.Add(mod);
    }

    public RequestDamageAmountEventArgs(IGameEntity target)
    {
        Target = target;
    }

    public int GetResult()
    {
        int total = 0;
        foreach (var mod in Modifiers)
        {
            total += mod.AdditiveModifier;
        }

        float totalPercentMod = 1;
        foreach (var mod in Modifiers)
        {
            totalPercentMod += mod.MultiplicativeModifier;
        }

        return (int)(total * totalPercentMod);
    }
}

public class DamageAmountModifier
{
    public int AdditiveModifier { get; set; }
    public float MultiplicativeModifier { get; set; }
}
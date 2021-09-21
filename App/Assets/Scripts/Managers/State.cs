using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;
using Scenes.Scripts.StateMachine;
using Object = UnityEngine.Object;

public abstract class State : IState<StateData>
{
    protected List<IDisposable> HighlightHandles { get; } = new List<IDisposable>();
    protected BattleProxy BattleProxy { get; set; }

    protected void ClearHighlights()
    {
        foreach (IDisposable handle in HighlightHandles)
        {
            handle.Dispose();
        }

        HighlightHandles.Clear();
    }

    protected IDisposable HighlightNode<T>(ActorNode pathnode) where T : EffectComponent
    {
        return BattleProxy.GetNodeProxyByEntity(pathnode)
            .GetComponentInChildren<T>(true)
            .GetEffectHandle();
    }

    public virtual void Enter(StateData input)
    {
        //TODO
        BattleProxy = Object.FindObjectOfType<BattleProxy>();
    }

    public virtual void Exit(StateData input)
    {
        ClearHighlights();
    }

    public abstract void EntitySelected(StateData input);
    public abstract void EntityHovered(StateData input);
    public abstract void EntityUnHovered(StateData input);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

public class DefaultBattleState : State
{
    public override void EntityUnHovered(StateData input)
    {
        ClearHighlights();
    }

    public override void EntitySelected(StateData input)
    {
        if (input.Selected is Card card)
        {
            IReadOnlyList<IGameEntity> validTargets = card.GetValidTargets();
            if (!card.IsPlayable() ||
                (card.RequiresTarget && !validTargets.Any())) //if its not playable or has no valid targets
            {
                //Negative visual feedback, i.e. card shake should play here.
                return;
            }


            InputManager.Instance.TransitionToState(InputManager.InputState.CardSelected);
        }
        else if (input.Selected is ActorNode node)
        {
            PlayerActor player = node.Context.GetCurrentBattle().Player;
            ActorNode playerNode = node.Context.GetCurrentBattle().Graph.GetNodeOfActor(player);
            ActorNodePath path =
                new ActorNodePath(playerNode, node, false, node.GetActor() == null);

            MoveToPath(player, path);
        }


        ClearHighlights();
    }

    private async Task MoveToPath(IActor actor, Path<ActorNode> path)
    {
        foreach (ActorNode node in path)
        {
            if (node.GetActor() == actor)
            {
                continue;
            }

            node.Context.GetCurrentBattle().Graph.MoveIntoSpace(actor, node);
            await Task.Delay(TimeSpan.FromSeconds(.25f));
        }
    }


    public override void Exit(StateData input)
    {
        ClearHighlights();
    }


    public override void EntityHovered(StateData input)
    {
        if (input.Selected is Card card)
        {
            IReadOnlyList<IGameEntity> targets = card.GetValidTargets();
            if (targets != null)
            {
                foreach (IGameEntity target in targets)
                {
                    if (target is ActorNode node)
                    {
                        HighlightNode<NodeHighlightThreatEffectComponent>(node);
                    }
                }
            }
        }
        else if (input.Selected is ActorNode node)
        {
            PlayerActor player = node.Context.GetCurrentBattle().Player;
            ActorNode playerNode = node.Context.GetCurrentBattle().Graph.GetNodeOfActor(player);
            ActorNodePath path =
                new ActorNodePath(playerNode, node, false, node.GetActor() == null);
            foreach (ActorNode pathnode in path)
            {
                HighlightNode<NodeHighlightPathEffectComponent>(pathnode);
            }
        }
        else if (input.Selected is Enemy enemy)
        {
            List<CubicHexCoord> affected = enemy.Intent.GetAffectedCoords();
            IReadOnlyDictionary<CubicHexCoord, ActorNode> nodes = BattleProxy.GameEntity.Graph.GetNodes();
            foreach (CubicHexCoord affectedNode in affected)
            {
                if (nodes.TryGetValue(affectedNode, out ActorNode entity))
                {
                    HighlightNode<NodeHighlightThreatEffectComponent>(entity);
                }
            }

            ActorNodeRange range = new ActorNodeRange(enemy.Context.GetCurrentBattle().Graph.GetNodeOfActor(enemy),
                enemy.MoveSpeed);
            foreach (var rangeNode in range.Nodes)
            {
                HighlightNode<NodeHighlightPathEffectComponent>(rangeNode);
            }
        }
    }

    public void HighlightNode<T>(ActorNode pathnode) where T : EffectComponent
    {
        HighlightHandles.Add(BattleProxy.GetNodeProxyByEntity(pathnode)
            .GetComponentInChildren<T>(true)
            .GetEffectHandle());
    }
}
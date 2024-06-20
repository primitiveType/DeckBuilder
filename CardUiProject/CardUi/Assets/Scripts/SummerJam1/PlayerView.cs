using System;
using Api;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class PlayerView : View<Player>, ISetModel, IGameObject
    {
        protected void Awake()
        {
            var game = GameContext.Instance.Context.Root.GetComponent<Game>();
            var player = game.Player;
            SetModel(player.Entity);
            var bridge = Entity.GetOrAddComponent<SummerJam1ModelViewBridge>();
            bridge.gameObject = gameObject;
            var component = Entity.GetComponent<IGameObject>();
            if (component.gameObject == null)
            {
                throw new NullReferenceException($"Player GO null somehow. {component.GetType()}.");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Logging.LogWarning("Player destroyed!");
        }
    }
}

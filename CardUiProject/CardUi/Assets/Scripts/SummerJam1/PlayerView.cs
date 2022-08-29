using System;
using App;

namespace SummerJam1
{
    public class PlayerView : View<Player>, ISetModel, IGameObject
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Context.Root.GetComponent<Game>().Player.Entity);
            var bridge = Entity.GetOrAddComponent<SummerJam1ModelViewBridge>();
            bridge.gameObject = gameObject;
            var component = Entity.GetComponent<IGameObject>();
            if (component.gameObject == null)
            {
                throw new NullReferenceException($"Player GO null somehow. {component.GetType()}.");
            }
        }
    }
}

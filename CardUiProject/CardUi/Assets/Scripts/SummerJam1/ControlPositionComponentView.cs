using App;
using App.Utility;
using CardsAndPiles.Components;
using SummerJam1;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlPositionComponentView : ComponentView<Position>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(0, 1, 0));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(-1, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(0, -1, 0 ));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(1, 0, 0));
        }
    }

    private void TryMoveTo(Vector3 newPosition)
    {
        var x = (int)newPosition.x;
        var y = (int)newPosition.y;

        if (x < 0 || x >= GameContext.Instance.Game.CurrentMap.Width)
        {
            return;
        }

        if (y < 0 || y >= GameContext.Instance.Game.CurrentMap.Height)
        {
            return;
        }

        var newCell = GameContext.Instance.Game.CurrentMap[x, y];
        Entity.TrySetParent(newCell.Entity);
    }

    protected override void ComponentOnPropertyChanged()
    {
    }
}

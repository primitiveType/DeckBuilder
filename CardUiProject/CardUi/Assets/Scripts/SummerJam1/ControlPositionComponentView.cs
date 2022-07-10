using App;
using App.Utility;
using CardsAndPiles.Components;
using SummerJam1;
using UnityEngine;

public class ControlPositionComponentView : ComponentView<Position>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(0, 0, 1));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(-1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(0, 0, -1));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryMoveTo(Component.Position1.ToUnityVector3() + new Vector3(1, 0, 0));
        }
    }

    private void TryMoveTo(Vector3 newPosition)
    {
        var newCell = SummerJam1Context.Instance.Game.CurrentMap.Map[(int)newPosition.x, (int)newPosition.z];
        if (newCell.IsWalkable)
        {
            Component.Position1 = newPosition.ToSystemVector3();
        }
    }

    protected override void ComponentOnPropertyChanged()
    {
            
    }
}

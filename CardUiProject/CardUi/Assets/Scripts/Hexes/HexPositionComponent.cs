using App;
using Wunderwunsch.HexMapLibrary;

public class HexPositionComponent : ComponentView<IHexPosition>
{
    protected override void ComponentOnPropertyChanged()
    {
        transform.position = Component.CartesianPosition;
    }
}
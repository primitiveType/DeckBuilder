using App.Utility;
using CardsAndPiles.Components;

namespace App
{
    public class LocalPositionComponentView : ComponentView<Position>
    {
        protected override void ComponentOnPropertyChanged()
        {
            transform.localPosition = Component.Position1.ToUnityVector3();
        }
    }

    
}

using System.ComponentModel;
using App;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class CameraFollowPositionComponent : View<IPosition>
    {
        [PropertyListener(nameof(IPosition.Pos))]
        private void OnPositionChanged(object sender, PropertyChangedEventArgs args)
        {
            CameraManager.Instance.ActivateFollowCamera();
        }
    }
}

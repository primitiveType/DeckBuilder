using System.ComponentModel;
using Api;
using App;
using SummerJam1.Units;
using UnityEngine;

namespace SummerJam1
{
    public class TransformView : View<TransformAfterTurns>
    {
        [PropertyListener(nameof(TransformAfterTurns.State))]
        private void ParentChanged(object sender, PropertyChangedEventArgs args){
            if (Model.State == LifecycleState.Destroyed)
            {
                Debug.Log("transformer removed!");
            }
        }
    }
}
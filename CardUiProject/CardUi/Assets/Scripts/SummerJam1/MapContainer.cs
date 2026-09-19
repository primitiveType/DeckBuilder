using System;
using Api;
using SummerJam1;

namespace App
{
    public class MapContainer : View<MapChoicesContainer>
    {
        private void Awake()
        {
            SetModel(GameContext.Instance.Game.MapChoicesContainer);
        }
    }
    
}

using System;
using Api;
using CardsAndPiles.Components;
using SummerJam1;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class CardInPrizePool : View<Card>, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SummerJam1Context.Instance.Game.PrizePile.ChoosePrize(Entity);
        }

    }
}
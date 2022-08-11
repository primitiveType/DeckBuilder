using System.Linq;
using App.Utility;
using CardsAndPiles;
using SummerJam1;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class ClickSendRandomToFirstEmptyEncounterSlot : MonoBehaviour, IPointerClickHandler
    {
        private IPileView PileView { get; set; }


        // Start is called before the first frame update
        void Start()
        {
            PileView = GetComponent<PileView>();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (PileView.Entity.Children.Count > 0)
            {
                IPileItem card = PileView.Entity.Children.GetRandom().GetComponent<IPileItem>();
                Pile pileViewToSendTo = GameContext.Instance.Game.Battle.EncounterSlots.First(slot => !slot.Entity.Children.Any());
                card.Entity.TrySetParent(pileViewToSendTo.Entity);
            }
        }
    }
}

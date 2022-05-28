using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSendRandomToPile : MonoBehaviour, IPointerClickHandler
{
    private IPile Pile { get; set; }

    [SerializeField] private Pile pileToSendTo; 
    // Start is called before the first frame update
    void Start()
    {
        Pile = GetComponent<Pile>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Pile.Items.Count > 0)
        {
            IPileItem card = Pile.Items.GetRandom();
            card.TrySendToPile(pileToSendTo);
        }
    }
}

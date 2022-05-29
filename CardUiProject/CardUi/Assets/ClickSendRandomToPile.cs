using CardsAndPiles;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSendRandomToPile : MonoBehaviour, IPointerClickHandler
{
    private IPileView PileView { get; set; }

    [SerializeField] private PileView m_PileViewToSendTo; 
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
            card.TrySendToPile(m_PileViewToSendTo.Model);
        }
    }
}

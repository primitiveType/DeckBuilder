using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class RemoveCardButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(RemoveCard);
        }

        private void RemoveCard()
        {
            GameContext.Instance.Events.OnRequestRemoveCard(new RequestRemoveCardEventArgs());
        }
    }
    
}

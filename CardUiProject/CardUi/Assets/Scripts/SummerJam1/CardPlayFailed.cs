using System.Collections;
using System.Linq;
using App;
using CardsAndPiles;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class CardPlayFailed : MonoBehaviour
    {
        [SerializeField] private TMP_Text Text;

        private Coroutine Coroutine { get; set; }

        private void Start()
        {
            GameContext.Instance.Events.SubscribeToCardPlayFailed(OnCardPlayFailed);
            gameObject.SetActive(false);
        }

        private void OnCardPlayFailed(object sender, CardPlayFailedEventArgs item)
        {
            Text.text = item.Reasons.FirstOrDefault();
            if (Coroutine != null)
            {
                CoroutineManager.Instance.EndCoroutine(Coroutine);
            }

            Coroutine = CoroutineManager.Instance.StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }
    }
    
    
}

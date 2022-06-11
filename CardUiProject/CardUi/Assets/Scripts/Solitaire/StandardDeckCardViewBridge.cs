using App;
using UnityEngine;

namespace Solitaire
{
    public class StandardDeckCardViewBridge : ViewBridge<StandardDeckCard, StandardDeckCardViewBridge>
    {
        public override GameObject Prefab => SolitaireHelper.Instance.CardPrefab;
    }
}
using System.Collections.Specialized;
using Api;
using Solitaire;
using UnityEngine;
using Component = Api.Component;
using Common;


public class StandardDeckCardViewBridge : ViewBridge<StandardDeckCard, StandardDeckCardViewBridge>
{
    public override GameObject Prefab => SolitaireHelper.Instance.CardPrefab;
}
using System.Collections;
using System.Linq;
using App.Utility;
using CardsAndPiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App
{
    [RequireComponent(typeof(ISortHandler))]
    public class PileItemView<T> : View<T> where T : IPileItem
    {
    }
}

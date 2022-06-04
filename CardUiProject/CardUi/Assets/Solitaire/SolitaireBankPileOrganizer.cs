using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Api;
using UnityEngine;

namespace Solitaire
{
    public class SolitaireBankPileOrganizer : PileOrganizer
    {
        private List<StandardDeckCardView> CardsInPile = new List<StandardDeckCardView>();
        [SerializeField] private float distanceBetweenCards = .5f;

        private void Update()
        {
            int index = 0;
            int count = CardsInPile.Count;
            foreach (StandardDeckCardView view in CardsInPile)
            {
                if (!view.IsDragging)
                {
                    view.SetLocalPosition(Vector3.down * distanceBetweenCards * index, new Vector3());
                    view.SortHandler.SetDepth((int)(Sorting.PileItem + index));
                }

                index++;
            }
        }

        protected override void ParentViewToPile(IEntity added)
        {
            base.ParentViewToPile(added);
            GameObject entityGO = added.GetComponent<IGameObject>()?.gameObject;

            if (entityGO != null)
            {
                foreach (StandardDeckCardView standardDeckCardView in CardsInPile)
                {
                    Destroy(standardDeckCardView.gameObject.GetComponent<DraggableComponent>());
                }

                CardsInPile.Add(entityGO.GetComponent<StandardDeckCardView>());
            }
        }

        protected override void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnPileChanged(sender, e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (IEntity removed in e.OldItems)
                    {
                        GameObject entityGO = removed.GetComponent<IGameObject>()?.gameObject;
                        if (entityGO != null)
                        {
                            StandardDeckCardView card = entityGO.GetComponent<StandardDeckCardView>();
                            CardsInPile.Remove(card);
                        }
                    }

                    CardsInPile.LastOrDefault()?.gameObject.AddComponent<DraggableComponent>();
                    break;
                }
            }
        }
    }
}
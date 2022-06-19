using System.Collections.Generic;
using System.Linq;
using Api;
using App;
using UnityEngine;

namespace Solitaire
{
    public class SolitaireBankPileOrganizer : PileOrganizer
    {
        private List<StandardDeckCardView> CardsInPile = new List<StandardDeckCardView>();
        [SerializeField] private float distanceBetweenCards = .5f;

        private void UpdatePositions()
        {
            int index = 0;
            int count = CardsInPile.Count;
            foreach (StandardDeckCardView view in CardsInPile)
            {
                if (!view.IsDragging)
                {
                    view.SetTargetPosition(Vector3.down * distanceBetweenCards * index, new Vector3(), true);
                    view.SortHandler.SetDepth((int)(Sorting.PileItem + index));
                }

                index++;
            }
        }

        protected override void OnItemAddedQueued(IEntity added)
        {
            base.OnItemAddedQueued(added);
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


        protected override void OnItemRemovedQueued(IEntity removed)
        {
            GameObject entityGO = removed.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null)
            {
                StandardDeckCardView card = entityGO.GetComponent<StandardDeckCardView>();
                CardsInPile.Remove(card);
                SetDirty();
            }
        }

        private void SetDirty()
        {
            IsDirty = true;
        }

        private bool IsDirty { get; set; }

        private void Update()
        {
            CardsInPile.LastOrDefault()?.gameObject.AddComponent<DraggableComponent>();
        }
    }
}
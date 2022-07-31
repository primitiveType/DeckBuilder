using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using UnityEngine;

namespace App
{
    public class HandPileOrganizer : PileOrganizer
    {
        [SerializeField] private float lerpRate = .01f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private float maxWidth = 1f;
        [SerializeField] private float overlapRatio = 1f;
        [SerializeField] private bool rotate = true;
        [SerializeField] private float offset = 0f;
        [SerializeField] private float hoveredOverlapRotation = 1f;
        [SerializeField] private float overlapReductionPerCard = .05f;

        private float CurrentOverlapRatio => overlapRatio - (overlapReductionPerCard * CardsInHand.Count);

        private List<CardInHand> CardsInHand { get; } = new List<CardInHand>();

        protected override void Start()
        {
            base.Start();
            ((CardEvents)PileView.Entity.Context.Events).SubscribeToTurnBegan(OnTurnBegan);
            ((CardEvents)PileView.Entity.Context.Events).SubscribeToTurnEnded(OnTurnEnded);
        }

        private bool IsPlayerTurn { get; set; } = true;

        private void OnTurnEnded(object sender, TurnEndedEventArgs item)
        {
            IsPlayerTurn = false;

            Disposables.Add(AnimationQueue.Instance.Enqueue((() => { IsPlayerTurn = false; })));
        }

        private void OnTurnBegan(object sender, TurnBeganEventArgs item)
        {
            Disposables.Add(AnimationQueue.Instance.Enqueue((() => { IsPlayerTurn = true; })));
        }


        private void Update()
        {
            PositionCards();
        }


        private void PositionCards()
        {
            float width = GetTotalWidth();
            float xPos = -width / 2f;
            int i = 0;
            foreach (CardInHand card in CardsInHand)
            {
                card.PileItemView.SortHandler?.SetDepth((int)Sorting.PileItem + i++);
                float halfSize = GetEffectiveCardWidth(card) / 2f;
                if (card.PileItemView.IsDragging)
                {
                    xPos += halfSize / 2f;
                    card.PileItemView.SortHandler?.SetDepth((int)Sorting.DraggedPileItem);
                    continue;
                }


                xPos += halfSize;
                float x;
                float y;
                if (rotate)
                {
                    float h = 0;
                    float k = -radius;
                    float theta = (Mathf.PI / 2f) + offset + (-xPos / radius);
                    x = radius * Mathf.Cos(theta) + h;
                    y = radius * Mathf.Sin(theta) + k;
                }
                else
                {
                    x = xPos;
                    y = 0;
                }

                Vector3 pileItemPosition = card.PileItemView.GetLocalPosition();
                Vector3 target = new Vector3(x, y, pileItemPosition.z);


                if (card.DisplayWholeCard)
                {
                    SetHoveredPosition(card, target, pileItemPosition);
                }
                else
                {
                    card.PileItemView.SetTargetPosition(target, GetRotation(xPos), IsPlayerTurn);
                }

                xPos += halfSize;
            }
        }

        private void SetHoveredPosition(CardInHand card, Vector3 target, Vector3 pileItemPosition)
        {
            //first move it to where it would be.
            card.PileItemView.SetLocalPosition(target, new Vector3());

            //then clamp it to the screen and update its transform position.
            Vector3 clampedPosition = card.PileItemView.GetBounds().ClampToViewport(Camera.main);
            Vector3 clampedLocalPosition =
                transform.InverseTransformPoint(clampedPosition).WithZ(pileItemPosition.z);
            card.PileItemView.SetTargetPosition(clampedLocalPosition, new Vector3(), IsPlayerTurn);

            card.PileItemView.SortHandler?.SetDepth((int)Sorting.DraggedPileItem);
            card.PileItemView.SetLocalPosition(pileItemPosition,
                new Vector3()); //reset its position to where it started.
        }

        private float GetEffectiveCardWidth(CardInHand card)
        {
            return 2 * card.PileItemView.GetBounds().extents.x *
                   (card.DisplayWholeCard ? hoveredOverlapRotation : CurrentOverlapRatio);
        }

        //theta = arclength/radius

        private Vector3 GetRotation(float xPos)
        {
            if (!rotate)
            {
                return Vector3.zero;
            }

            return new Vector3(0, 0, Mathf.Rad2Deg * (offset + (-xPos / radius)));
        }

        private float GetTotalWidth()
        {
            float width = 0;

            foreach (CardInHand card in CardsInHand.Where(item => !item.PileItemView.IsDragging))
            {
                width += GetEffectiveCardWidth(card);
            }

            return width;
        }


        protected override void OnItemRemovedQueued(IEntity removed)
        {
            base.OnItemRemovedQueued(removed);
            GameObject entityGO = removed.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null)
            {
                CardInHand card = entityGO.GetComponent<CardInHand>();
                CardsInHand.Remove(card);
                Destroy(card);
            }
        }

        protected override async Task OnItemAddedQueued(IEntity added)
        {
            await base.OnItemAddedQueued(added);
            GameObject entityGO = added.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null && entityGO.GetComponent<CardInHand>() != null)
            {
//                Debug.LogError($"{entityGO.name} was already in hand!?");
            }

            if (entityGO != null)
            {
                CardsInHand.Add(entityGO.AddComponent<CardInHand>());
            }
        }
    }
}

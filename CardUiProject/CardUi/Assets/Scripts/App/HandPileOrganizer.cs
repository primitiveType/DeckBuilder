using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Api;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace App
{
    public class HandPileOrganizer : PileOrganizer
    {
        [SerializeField] private float radius = 1f;
        [SerializeField] private Vector2 overlapRatio = new Vector2(1f, 0);
        [SerializeField] private bool rotate = true;
        [SerializeField] private bool reverseDepthSort;
        [SerializeField] private float radialOffset = 0f;
        [SerializeField] private Vector2 hoveredOverlapRatio = new Vector2(1f, 0f);
        [SerializeField] private Vector2 overlapReductionPerCard = new Vector2(.05f, 0);

        private Vector2 CurrentOverlapRatio => overlapRatio - (overlapReductionPerCard * CardsInHand.Count);

        private List<CardInHand> CardsInHand { get; } = new List<CardInHand>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ((CardEvents)Entity.Context.Events).SubscribeToTurnBegan(OnTurnBegan);
            ((CardEvents)Entity.Context.Events).SubscribeToTurnEnded(OnTurnEnded);
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
            float height = GetTotalHeight();
            float xPos = -width / 2f;
            float yPos = -height / 2f;
            int i = 0;
            int depthMultiplier = reverseDepthSort ? -1 : 1;
            foreach (CardInHand card in CardsInHand)
            {
                card.PileItemView.SortHandler?.SetDepth((int)Sorting.PileItem + i++ * depthMultiplier);
                float halfWidth = GetEffectiveCardWidth(card) / 2f;
                float halfHeight = GetEffectiveCardHeight(card) / 2f;
                if (card.PileItemView.IsDragging)
                {
                    xPos += halfWidth / 2f;
                    card.PileItemView.SortHandler?.SetDepth((int)Sorting.DraggedPileItem * depthMultiplier);
                    continue;
                }


                xPos += halfWidth;
                yPos += halfHeight;
                float x;
                float y;
                if (rotate)
                {
                    float h = 0;
                    float k = -radius;
                    float theta = (Mathf.PI / 2f) + radialOffset + (-xPos / radius);
                    x = radius * Mathf.Cos(theta) + h;
                    y = radius * Mathf.Sin(theta) + k;
                }
                else
                {
                    x = xPos;
                    y = yPos;
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

                xPos += halfWidth;
                yPos += halfHeight;
            }
        }

        private float GetEffectiveCardHeight(CardInHand card)
        {
            return 2 * card.PileItemView.GetBounds().extents.y *
                   (card.DisplayWholeCard ? hoveredOverlapRatio.y : CurrentOverlapRatio.y);
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
                   (card.DisplayWholeCard ? hoveredOverlapRatio.x : CurrentOverlapRatio.x);
        }

        //theta = arclength/radius

        private Vector3 GetRotation(float xPos)
        {
            if (!rotate)
            {
                return Vector3.zero;
            }

            return new Vector3(0, 0, Mathf.Rad2Deg * (radialOffset + (-xPos / radius)));
        }

        private float GetTotalHeight()
        {
            float height = 0;

            foreach (CardInHand card in CardsInHand.Where(item => !item.PileItemView.IsDragging))
            {
                height += GetEffectiveCardHeight(card);
            }

            return height;
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

        protected override async Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            await base.OnItemAddedQueued(added, view);
            GameObject entityGO = added.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null && entityGO.GetComponent<CardInHand>() != null)
            {
//                Logging.LogError($"{entityGO.name} was already in hand!?");
            }

            if (entityGO != null)
            {
                CardsInHand.Add(entityGO.AddComponent<CardInHand>());
            }
        }
    }
}

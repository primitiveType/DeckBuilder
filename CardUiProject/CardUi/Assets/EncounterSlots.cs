using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using App;
using CardsAndPiles;
using RandN.Rngs;
using SummerJam1;
using UnityEngine;
using UnityEngine.EventSystems;

// public class EncounterSlots : View<BattleContainer>, IDragHandler, IEndDragHandler, IBeginDragHandler
// {
//     [SerializeField] private Vector2 m_Spacing = new Vector2(5, 9);
//
//     [SerializeField] private EncounterSlotPileView m_Prefab;
//     [SerializeField] private Transform m_Parent;
//
//     private float XOffset { get; set; }
//
//     public float YOffset { get; set; }
//
//     private Camera Camera { get; set; }
//
//     private bool Dragging { get; set; }
//
//     private float LastPosition { get; set; }
//
//     // Start is called before the first frame update
//     void Awake()
//     {
//         GameContext.Instance.Events.SubscribeToDungeonPhaseEnded(OnDungeonPhaseEnd);
//         GameContext.Instance.Game.Battle.PropertyChanged += BattleOnPropertyChanged;
//         Camera = Camera.main;
//         float cardWidth = 2.5f;
//         XOffset = cardWidth - ((m_Spacing.x * BattleContainer.NumEncounterSlotsPerFloor) / 2f);
//         SetModel(GameContext.Instance.Game.Battle);
//         int created = 0;
//         foreach (KeyValuePair<int, List<Pile>> battleAllEncounterSlot in Model.Encount)
//         {
//             foreach (Pile pile in battleAllEncounterSlot.Value)
//             {
//                 EncounterSlotPileView go = Instantiate(m_Prefab, m_Parent);
//                 go.SetModel(pile);
//                 go.transform.localPosition = new Vector3(m_Spacing.x * (created % BattleContainer.NumEncounterSlotsPerFloor) + XOffset,
//                     m_Spacing.y * (created / BattleContainer.NumEncounterSlotsPerFloor));
//                 created++;
//             }
//         }
//     }
//
//     private void BattleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
//     {
//         if (e.PropertyName == nameof(BattleContainer.CurrentFloor))
//         {
//             m_Parent.localPosition = new Vector3(m_Parent.localPosition.x, -m_Spacing.y * GameContext.Instance.Game.Battle.CurrentFloor,
//                 m_Parent.localPosition.z);
//         }
//     }
//
//     private void OnDungeonPhaseEnd(object sender, DungeonPhaseEndedEventArgs item)
//     {
//         transform.localScale = Vector3.one;
//     }
//
//     private void Update()
//     {
//         return;
//         m_Parent.localPosition = new Vector3(XOffset, YOffset, m_Parent.localPosition.z);
//     }
//
//     public void OnDrag(PointerEventData eventData)
//     {
//         float diff = eventData.delta.y; //pixels
//
//         float pixelsPerMeter = Screen.height / (Camera.orthographicSize * 2);
//
//
//         YOffset += diff / pixelsPerMeter;
//     }
//
//     public void OnEndDrag(PointerEventData eventData)
//     {
//         Dragging = false;
//     }
//
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         Dragging = true;
//         LastPosition = eventData.pointerCurrentRaycast.worldPosition.y;
//     }
// }

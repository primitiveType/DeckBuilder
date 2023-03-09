﻿using System;
using System.Collections.Generic;
using Api;
using App;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Units;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class ViewFactory : MonoBehaviourSingleton<ViewFactory>
    {
        [SerializeField] private GameObject m_CardPrefab;
        [SerializeField] private GameObject m_RelicPrefab;
        [SerializeField] private GameObject m_UnitPrefab;

        [SerializeField] private GameObject m_WalkablePrefab;
        [SerializeField] private GameObject m_PrefabReference;
        [SerializeField] private GameObject m_WallPrefab;
        [SerializeField] private GameObject m_DungeonPrefab;
        [SerializeField] private GameObject m_HatchEncounterPrefab;
        [SerializeField] private GameObject m_PlayerPrefab;

        public GameObject m_DefaultParent;

        private List<IDisposable> Disposables { get; } = new List<IDisposable>();
        public GameObject CardPrefab => m_CardPrefab;
        private GameObject RelicPrefab => m_RelicPrefab;
        private GameObject UnitPrefab => m_UnitPrefab;

        private GameObject WalkablePrefab => m_WalkablePrefab;

        private GameObject WallPrefab => m_WallPrefab;


        protected override void SingletonStarted()
        {
            base.SingletonStarted();

            Disposables.Add(GameContext.Instance.Events.SubscribeToEntityCreated(OnEntityCreated));
            CreateViewsForExistingModels();
        }

        private void CreateViewsForExistingModels()
        {
            List<IVisual> existing = GameContext.Instance.Context.Root.GetComponentsInChildren<IVisual>();
            foreach (IVisual child in existing)
            {
                GameObject prefab = GetPrefab(child);
                if (prefab != null)
                {
                    CreateView(((IComponent)child).Entity, prefab);
                }
            }
        }

        private void OnEntityCreated(object sender, EntityCreatedEventArgs item)
        {
            CreateGameObjectForModel(item.Entity);
        }


        private void OnRelicCreated(object sender, RelicCreatedEventArgs args)
        {
            IEntity entity = args.Relic;
            CreateView(entity, RelicPrefab);
        }


        private void OnCardCreated(object sender, CardCreatedEventArgs args)
        {
            IEntity entity = args.CardId;
            CreateView(entity, CardPrefab);
        }

        public GameObject CreateGameObjectForModel(IEntity entity)
        {
            IVisual visual = entity.GetComponent<IVisual>();
            GameObject prefab = GetPrefab(visual);
            return prefab != null ? CreateView(entity, prefab) : null;
        }

        public GameObject GetPrefab(IVisual visual)
        {
            switch (visual)
            {
                case RelicEncounter relicEncounter:
                    break;
                case ShrineEncounter shrineEncounter:
                    Debug.Log("Found shrine encounter.");
                    break;
                case RelicComponent relic:
                    return RelicPrefab;
                case Unit unit:
                    return UnitPrefab;
                case Card card:
                    return CardPrefab;
                case DungeonPile dungeonPile:
                    return m_DungeonPrefab;
                case PrefabReference prefabReference:
                    return m_PrefabReference;
                default:
                    // throw new ArgumentOutOfRangeException(nameof(visual));
                    Debug.LogWarning("No prefab visual found.");
                    return null;
            }


            return null;
        }

        public static GameObject CreateView(IEntity entity, GameObject prefab)
        {
            GameObject unitView = Instantiate(prefab);
            // unitView.transform.localPosition = Vector3.one * 10_000;
            unitView.GetComponent<ISetModel>().SetModel(entity);
            entity.GetOrAddComponent<SummerJam1ModelViewBridge>().gameObject = unitView;
            unitView.transform.SetParent(Instance.m_DefaultParent.transform);
            return unitView;
        }
    }
}

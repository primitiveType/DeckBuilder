using System;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using UnityEngine;
using UnityEngine.UI;

public class EdgeProxy : GameEntityComponent //edges are not entities
{
    [SerializeField] private NodeProxy NodeA;
    [SerializeField] private NodeProxy NodeB;


    [SerializeField] private Text AbText;
    [SerializeField] private Text BaText;

    private LineRenderer LineRenderer { get; set; }

    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.endWidth = .1f;
        LineRenderer.startWidth = .1f;
        LineRenderer.SetPositions(new[] { NodeA.transform.position, NodeB.transform.position });
        Context.Events.IntentChanged += OnIntentChanged;
        UpdateVisual();
    }

    private void OnDestroy()
    {
        Context.Events.IntentChanged -= OnIntentChanged;
    }

    private void OnIntentChanged(object sender, IntentChangedEventArgs args)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        LineRenderer.startColor = LineRenderer.endColor = Color.white;
        UpdateVisual(NodeA.GameEntity.Actor as Enemy);
        UpdateVisual(NodeB.GameEntity.Actor as Enemy);
    }

    private void UpdateVisual(Enemy owner)
    {
        if (owner == null || owner.Intent == null)
        {
            return;
        }

        if (owner == NodeA.GameEntity?.Actor && owner.Intent.Target == NodeB.GameEntity)
        {
            LineRenderer.startColor = Color.red;
        }

        if (owner == NodeB.GameEntity?.Actor && owner.Intent.Target == NodeA.GameEntity)
        {
            LineRenderer.endColor = Color.red;
        }
    }
}
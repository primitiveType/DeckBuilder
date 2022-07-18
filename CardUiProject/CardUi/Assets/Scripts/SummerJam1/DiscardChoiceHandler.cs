using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Api;
using App;
using CardsAndPiles;
using SummerJam1;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscardChoiceHandler : PileView, IPileView
{
    public TMP_Text _HeaderText;

    [SerializeField] private Button m_SubmitButton;

    private readonly List<GameObject> ModifiedCards = new List<GameObject>();

    private int AmountExpected { get; set; }

    protected void Awake()
    {
        base.Start();
        SetModel(SummerJam1Context.Instance.Game.DiscardStagingPile.Entity);
        Disposables.Add(SummerJam1Context.Instance.Events.SubscribeToChooseCardsToDiscard(OnChooseCardsToDiscard));
        gameObject.SetActive(false);
        Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        m_SubmitButton.onClick.AddListener(Submit);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
    }

    private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateSubmitButton();
    }

    private void UpdateSubmitButton()
    {
        m_SubmitButton.interactable = Entity.Children.Count == AmountExpected;
    }

    private void OnChooseCardsToDiscard(object sender, ChooseCardsToDiscardEventArgs item)
    {
        InputStateManager.Instance.StateMachine.Fire(InputAction.ChooseDiscard);
        AmountExpected = item.Amount;
        gameObject.SetActive(true);
        string s = item.Amount > 1 ? "s" : "";
        _HeaderText.text = $"Discard {item.Amount} card{s}";
        UpdateSubmitButton();
        foreach (IEntity child in SummerJam1Context.Instance.Game.Battle.Hand.Entity.Children)
        {
            GameObject go = child.GetComponent<IGameObject>().gameObject;
            ModifiedCards.Add(go);
            go.AddComponent<ClickAddToDiscardStaging>();
        }
    }

    private void CleanupModifiedCards()
    {
        foreach (GameObject modifiedCard in ModifiedCards)
        {
            Destroy(modifiedCard.GetComponent<ClickAddToDiscardStaging>());
        }

        ModifiedCards.Clear();
    }

    private void Submit()
    {
        foreach (IEntity child in Entity.Children.ToList())
        {
            child.TrySetParent(SummerJam1Context.Instance.Game.Battle.Discard);
        }

        gameObject.SetActive(false);
        CleanupModifiedCards();
        InputStateManager.Instance.StateMachine.Fire(InputAction.EndChooseDiscard);
    }
}

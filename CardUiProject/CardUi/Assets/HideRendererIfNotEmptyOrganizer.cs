using Api;
using App;
using UnityEngine;

public class HideRendererIfNotEmptyOrganizer : PileOrganizer
{
    [SerializeField] private GameObject ToHide;

    protected override void Start()
    {
        base.Start();
        SetActive();
    }

    protected override void OnItemAddedQueued(IEntity added)
    {
        base.OnItemAddedQueued(added);
        SetActive();
    }

    protected override void OnItemRemovedQueued(IEntity removed)
    {
        base.OnItemRemovedQueued(removed);
        SetActive();
    }

    private void SetActive()
    {
        ToHide.SetActive(PileView.Entity.Children.Count == 0);
    }
}

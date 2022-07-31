using System.Threading.Tasks;
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

    protected override async Task OnItemAddedQueued(IEntity added)
    {
        await base.OnItemAddedQueued(added);
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

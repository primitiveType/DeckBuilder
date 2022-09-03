using System.Threading.Tasks;
using Api;
using App;
using UnityEngine;

public class HideRendererIfNotEmptyOrganizer : PileOrganizer
{
    [SerializeField] private GameObject ToHide;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetActive();
    }

    protected override async Task OnItemAddedQueued(IEntity added, IGameObject view)
    {
        await base.OnItemAddedQueued(added, view);
        SetActive();
    }

    protected override void OnItemRemovedQueued(IEntity removed)
    {
        base.OnItemRemovedQueued(removed);
        SetActive();
    }

    private void SetActive()
    {
        ToHide.SetActive(Entity.Children.Count == 0);
    }
}

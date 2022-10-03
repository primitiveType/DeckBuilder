using System.Threading.Tasks;
using Api;
using App;
using UnityEngine;

public class HideRendererIfNotEmptyOrganizer : PileOrganizer
{
    [SerializeField] private GameObject ToHide;
    [SerializeField] private bool inverse;

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
        if (inverse)
        {
            ToHide.SetActive(Entity.Children.Count != 0);
        }
        else
        {
            ToHide.SetActive(Entity.Children.Count == 0);
        }
    }
}

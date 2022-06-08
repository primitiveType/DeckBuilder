using System.Collections;
using System.Collections.Generic;
using Api;
using App;
using CardsAndPiles;
using Common;
using UnityEngine;

public class DisableColliderUntilTurnStart : View<IComponent>
{
    [SerializeField] private Collider m_Collider;
    // Start is called before the first frame update
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ((CardEvents)Entity.Context.Events).SubscribeToTurnBegan(Enable);
        ((CardEvents)Entity.Context.Events).SubscribeToTurnEnded(Disable);
    }

    private void Disable(object sender, TurnEndedEventArgs item)
    {
        AnimationQueue.Instance.Enqueue(DisableCR);
        m_Collider.enabled = false;
    }

    private IEnumerator DisableCR()
    {
        m_Collider.enabled = false;
        yield return null;
    }

    private IEnumerator EnableCR()
    {
        m_Collider.enabled = true;
        yield return null;
    }

    private void Enable(object sender, TurnBeganEventArgs item)
    {
        AnimationQueue.Instance.Enqueue(EnableCR);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

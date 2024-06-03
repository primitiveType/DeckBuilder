using Api;
using App;
using CardsAndPiles;
using UnityEngine;

public class DisableColliderUntilTurnStart : View<IComponent>
{
    [SerializeField] private Collider m_Collider;
    // Start is called before the first frame update
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Disposables.Add(((CardEvents)Entity.Context.Events).SubscribeToTurnBegan(Enable));
        Disposables.Add(((CardEvents)Entity.Context.Events).SubscribeToTurnEnded(Disable));
    }

    private void Disable(object sender, TurnEndedEventArgs item)
    {
        Disposables.Add(AnimationQueue.Instance.Enqueue(DisableCR));
        m_Collider.enabled = false;
    }

    private void DisableCR()
    {
        m_Collider.enabled = false;
        
    }

    private void EnableCR()
    {
        m_Collider.enabled = true;
    }

    private void Enable(object sender, TurnBeganEventArgs item)
    {
        Disposables.Add(AnimationQueue.Instance.Enqueue(EnableCR));

    }

    
}

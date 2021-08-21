using UnityEngine;
using UnityEngine.UI;

public class PlayerActorProxy : ActorProxy
{
    [SerializeField] private Text m_HealthText;
    private Text HealthText => m_HealthText;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        UpdateText();
    }

    public override void DamageReceived()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = $"Health : {GameEntity.Health}";
    }
}
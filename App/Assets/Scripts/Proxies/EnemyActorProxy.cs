using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using UnityEngine;
using UnityEngine.UI;

public class EnemyActorProxy : ActorProxy<Actor>
{
    [SerializeField] private Text HealthText;

    [SerializeField] private Text ArmorText;

    [SerializeField] private IntentProxy IntentProxyPrefab;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        CreateIntentProxy();


        GameEntity.Context.Events.IntentChanged += OnIntentChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEntity.Context.Events.IntentChanged -= OnIntentChanged;
    }

    private void OnIntentChanged(object sender, IntentChangedEventArgs args)
    {
        if (args.Owner == GameEntity)
        {
            CreateIntentProxy();
        }
    }

    private void CreateIntentProxy()
    {
        if (GameEntity is Enemy enemy)
        {
            if (enemy.Intent?.Target != null)
            {
                NodeProxy node = GetComponentInParent<BattleProxy>()
                    .GetNodeProxyByEntity(enemy.Intent.Target as ActorNode); //assumes nodes always target
                IntentProxy intentProxy = Instantiate(IntentProxyPrefab, node.Visual.transform, false);
                intentProxy.transform.localRotation = Quaternion.identity;
                intentProxy.transform.localPosition = Vector3.up * .01f;

                intentProxy.Initialize(enemy.Intent);
            }
        }
    }

    private void Start()
    {
        UpdateText();
    }


    public override void DamageReceived()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = $"Health: {GameEntity.Health.ToString()}";
        ArmorText.text = $"Armor: {GameEntity.Armor.ToString()}";
    }
}
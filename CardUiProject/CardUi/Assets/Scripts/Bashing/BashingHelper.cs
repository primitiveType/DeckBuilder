using System;
using System.Collections.Generic;
using System.IO;
using Api;
using App;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = Api.Component;

public class BashingHelper : MonoBehaviourSingleton<BashingHelper>
{
    [SerializeField] private GameObject m_CardPrefab;
    [SerializeField] private PileView m_PilePrefab;
    private PileView PlayArea;
    private Context Context { get; set; }
    private CardEvents Events => (CardEvents)Context.Events;
    [SerializeField] private TMP_InputField SaveInput;
    private List<IDisposable> Disposables { get; } = new List<IDisposable>();

    protected override void SingletonAwakened()
    {
        base.SingletonAwakened();
        Context = new Context(new CardEvents());
        AttachToContext();
    }

    private void AttachToContext()
    {
        Disposables.Add(Events.SubscribeToCardCreated(OnCardCreated));
        Context.CreateEntity(Context.Root, entity =>
        {
            entity.AddComponent<DeckPile>();
            PlayArea = Instantiate(m_PilePrefab);
            PlayArea.gameObject.SetActive(true);
            PlayArea.SetModel(entity);
        });
        foreach (Card card in Context.Root.GetComponentsInChildren<Card>())
        {
            CreateView(card.Entity, m_CardPrefab);
        }
    }

    private void Cleanup()
    {
        foreach (IDisposable disposable in Disposables)
        {
            disposable.Dispose();
        }

        Disposables.Clear();
    }

    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveInput.text + ".json");
        File.WriteAllText(path, Serializer.Serialize(Context));
    }

    public void Load()
    {
        Cleanup();
        Context.Root.Destroy();
        Context = null;

        string path = Path.Combine(Application.persistentDataPath, SaveInput.text + ".json");
        Context = Serializer.Deserialize<Context>(File.ReadAllText(path));

        AttachToContext();
    }

    public void MakeCard()
    {
        Context.CreateEntity(Context.Root, (entity) =>
        {
            entity.AddComponent<DoNothingCard>();
            entity.AddComponent<DescriptionComponent>();
            entity.AddComponent<NameComponent>();
            entity.AddComponent<Position>();
            entity.AddComponent<Draggable>().CanDrag = true;
        });
    }

    private void OnCardCreated(object sender, CardCreatedEventArgs item)
    {
        CreateView(item.CardId, m_CardPrefab).transform.position = Vector3.zero;
    }

    public static GameObject CreateView(IEntity entity, GameObject prefab)
    {
        GameObject unitView = Instantiate(prefab);
        unitView.GetComponent<ISetModel>().SetModel(entity);
        entity.GetOrAddComponent<BashingModelViewBridge>().gameObject = unitView;

        return unitView;
    }


    public GameObject CreateGameObjectForModel(Component summerJam1ModelViewBridge)
    {
        if (summerJam1ModelViewBridge.Entity.GetComponent<Card>() != null)
        {
            return CreateView(summerJam1ModelViewBridge.Entity, SummerJam1CardFactory.Instance.CardPrefab);
        }

        return null;
    }
}

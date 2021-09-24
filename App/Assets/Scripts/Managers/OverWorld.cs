using System;
using ca.axoninteractive.Geometry.Hex;
using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Terrain;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverWorld : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Button Button2;

    [SerializeField] private Button CreateDeckButton;

    private IContext Context => GameContextManager.Instance.Context;

    private void Awake()
    {

        Debug.Log("Entered Overworld with num cards: " + Context.PlayerDeck.Count);
        Context.PlayerDeck.Add(Context.CreateEntity<Attack5Damage>());
        //if (Context.PlayerDeck.Count == 0)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Context.PlayerDeck.Add(Context.CreateEntity<TargetsCards>());
        //        Context.PlayerDeck.Add(Context.CreateEntity<TestDiscover>());



        //        if (i % 2 == 0)
        //        {
        //            Context.PlayerDeck.Add(Context.CreateEntity<SecondChance>());
        //            Context.PlayerDeck.Add(Context.CreateEntity<Attack5Damage>());
        //            Context.PlayerDeck.Add(Context.CreateEntity<MoveToEmptyAdjacentNode>());
        //        }
        //        else
        //        {
        //            Context.PlayerDeck.Add(Context.CreateEntity<Attack5DamageAdjacentAlt>());
        //            Context.PlayerDeck.Add(Context.CreateEntity<Attack10DamageExhaust>());
        //        }

        //        Context.PlayerDeck.Add(Context.CreateEntity<DoubleNextCardDamage>());
        //    }

        //    //   Context.PlayerDeck.Add(Context.CreateEntity<Defend>());
        //    //     Context.PlayerDeck.Add(Context.CreateEntity<Anger>());
        //}

        //I imagine in the long term, the "buttons" here will be dynamically added in some fashion, and hooked up to pre-designed battle datas.    
        Button.onClick.AddListener(Test);
        Button2.onClick.AddListener(Test2);

        CreateDeckButton.onClick.AddListener(CreateDeck);
    }

    private void Test()
    {
        SceneManager.LoadSceneAsync("BattleScene").completed += delegate(AsyncOperation operation)
        {
            Context.StartBattle(Tools.Player, Context.CreateEntity<RandomBattleData>());
        };
    }

    private void Test2()
    {
        // Context.StartBattle(Tools.Player, Context.CreateEntity<FiveSlotBattleData>());
        SceneManager.LoadScene("BattleScene");
    }

    private void CreateDeck()
    {
       // Context.PlayerDeck.Clear();
        //foreach (Type type in Assembly.GetAssembly(typeof(EnergyCard)).GetTypes().AsEnumerable().Where(x => typeof(Card).IsAssignableFrom(x)))
        //{
        //    if (!type.IsAbstract)
        //    {
        //        Context.PlayerDeck.Add((Card)Context.CreateEntity(type));
        //    }

        //}

        Context.StartBattle(Tools.Player, Context.CreateEntity<BasicBattleData>());
        SceneManager.LoadScene("DeckScene");
    }
}


public class RandomBattleData : BattleData<HexGraph>
{
    private int width => 50;
    private int height => 50;

    private Actor Player { get; set; }

    public override void PrepareBattle(Actor player)
    {
        Player = player;
        float[,] noiseValues = new float[width, height];
        int seed = DateTime.Now.Second;
        float increment = .1f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noiseValues[i, j] = Mathf.PerlinNoise(increment * (i), increment * (j));
                AddTile(i, j, noiseValues[i, j]);
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
            }
        }
    }

    private bool playerPlaced;

    private void AddTile(int x, int y, float noiseValue)
    {
        if (Graph.TryGetNode(new AxialHexCoord(x, y).ToCubic(), out var node))
        {
            if (noiseValue > .65)
            {
                node.AddEntityNoEvent(Context.CreateEntity<Collectible>());
            }
            else if (noiseValue > .6)
            {
                node.AddEntityNoEvent(Context.CreateEntity<BlockedTerrain>());
            }
            else if (noiseValue < .2)
            {
                node.AddEntityNoEvent(Context.CreateEntity<FireTerrain>());
            }
            else if (!playerPlaced)
            {
                node.AddEntityNoEvent(Player);
                playerPlaced = true;
            }
        }
    }
}
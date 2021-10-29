using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
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
        Context.StartBattle(Tools.Player, Context.CreateEntity<FunBattleData>());
        //need to figure out how we are going to pass battle data to the scene.
        SceneManager.LoadScene("BattleScene");
    }

    private void Test2()
    {
        // Context.StartBattle(Tools.Player, Context.CreateEntity<FiveSlotBattleData>());
        //need to figure out how we are going to pass battle data to the scene.v
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
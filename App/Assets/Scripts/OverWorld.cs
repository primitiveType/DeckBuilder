using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverWorld : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Button Button2;

    private IContext Context => GameContextManager.Instance.Context;

    private void Awake()
    {
        if (Context.PlayerDeck.Count == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                Context.PlayerDeck.Add(Context.CreateEntity<TargetsCards>());
                if (i % 2 == 0)
                {
                    Context.PlayerDeck.Add(Context.CreateEntity<SecondChance>());
                    Context.PlayerDeck.Add(Context.CreateEntity<Attack5Damage>());
                    Context.PlayerDeck.Add(Context.CreateEntity<MoveToEmptyAdjacentNode>());
                }
                else
                {
                    Context.PlayerDeck.Add(Context.CreateEntity<Attack5DamageAdjacent>());
                    Context.PlayerDeck.Add(Context.CreateEntity<Attack10DamageExhaust>());
                }

                Context.PlayerDeck.Add(Context.CreateEntity<DoubleNextCardDamage>());
            }
        }

        //I imagine in the long term, the "buttons" here will be dynamically added in some fashion, and hooked up to pre-designed battle datas.    
        Button.onClick.AddListener(Test);
        Button2.onClick.AddListener(Test2);
    }

    private void Test()
    {
        Context.StartBattle(Tools.Player, Context.CreateEntity<BasicBattleData>());
        //need to figure out how we are going to pass battle data to the scene.
        SceneManager.LoadScene("BattleScene");
    }

    private void Test2()
    {
        Context.StartBattle(Tools.Player, Context.CreateEntity<FiveSlotBattleData>());
        //need to figure out how we are going to pass battle data to the scene.v
        SceneManager.LoadScene("BattleScene");
    }
}
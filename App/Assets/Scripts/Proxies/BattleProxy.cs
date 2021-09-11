using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BattleProxy : Proxy<IBattle>
{
    [SerializeField] private GameObject m_BattleEndGameObject;
    [SerializeField] private GameObject m_VictoryGameObject;
    [SerializeField] private GameObject m_LossGameObject;
    [SerializeField] private Button m_LeaveBattleButton;
    private GameObject VictoryGameObject => m_VictoryGameObject;
    private GameObject LossGameObject => m_LossGameObject;
    private Button LeaveBattleButton => m_LeaveBattleButton;
    private GameObject BattleEndGameObject => m_BattleEndGameObject;

    private IContext Api => GameEntity.Context;
    private IGameEvents GameEventHandler => Api.Events;


    protected override void OnInitialize()
    {
        LeaveBattleButton.onClick.AddListener(LeaveBattle);
        GameEventHandler.BattleEnded += OnBattleEnded;
    }


    private void LeaveBattle()
    {
        SceneManager.LoadScene("Overworld");//TODO
    }

    private void OnBattleEnded(object sender, BattleEndedEventArgs args)
    {
        BattleEndGameObject.SetActive(true);
        VictoryGameObject.SetActive(args.IsVictory);
        LossGameObject.SetActive(!args.IsVictory);
    }

    private void OnDestroy()
    {
        GameEventHandler.BattleEnded -= OnBattleEnded;
    }

    public abstract NodeProxy GetNodeProxyByEntity(ActorNode entity);
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App
{
  public class SummerJam1Menu : MonoBehaviour
  {
    public void StartBattle()
    {
      SceneManager.LoadScene("Scenes/SummerJam1/BattleScene");
    }

 
  }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Awake()
    {
        // GlobalApi.Initialize();

        //Temp code
        SceneManager.LoadScene("BattleScene");
    }
}
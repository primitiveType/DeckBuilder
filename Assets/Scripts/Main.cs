using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Awake()
    {
        Injector.Initialize();
        SceneManager.LoadScene("BattleScene");
    }
}
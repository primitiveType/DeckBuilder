using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
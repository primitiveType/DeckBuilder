using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnStart : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void Start()
    {
        SceneManager.LoadScene(SceneName);
    }
}
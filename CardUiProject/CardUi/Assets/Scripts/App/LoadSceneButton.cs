using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(SceneName));
    }
}

using System;
using System.Threading.Tasks;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {
        TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        SceneManager.LoadScene("MainMenu");

        Tools.Initialize(new PlayerPrefsData());
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Debug.LogError($"Caught exception in unobserved task! {e.Exception}");
    }
}
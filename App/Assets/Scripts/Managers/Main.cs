using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {
        TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        SceneManager.LoadScene("Overworld");

        Tools.Initialize(new PlayerPrefsData());
        
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Debug.LogError($"Caught exception in unobserved task! {e.Exception}");
    }
}
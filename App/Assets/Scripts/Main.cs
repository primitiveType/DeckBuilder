using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Overworld");

        Tools.Initialize(new PlayerPrefsData());
        
    }
}
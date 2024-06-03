using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MaterialRandomFloatOnSceneLoad : MonoBehaviour
{
    private Graphic CurrentRenderer;

    [SerializeField] private string PropertyName;

    // Start is called before the first frame update
    void Start()
    {
        CurrentRenderer = GetComponent<Graphic>();
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        SetProperty();
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SetProperty();
    }

    private void SetProperty()
    {
        CurrentRenderer.material.SetFloat(PropertyName, Random.Range(0f, 1f));
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
    }
}

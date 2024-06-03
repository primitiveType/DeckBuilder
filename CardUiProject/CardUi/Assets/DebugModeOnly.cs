using System;
using System.ComponentModel;
using UnityEngine;

public class DebugModeOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateVisibility();
        DebugMode.Instance.PropertyChanged += InstanceOnPropertyChanged;
    }

    private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        UpdateVisibility();
    }

    void UpdateVisibility()
    {
        gameObject.SetActive(DebugMode.Instance.IsDebug);
    }

    private void OnDestroy()
    {
        if (DebugMode.Instance != null)
        {
            DebugMode.Instance.PropertyChanged -= InstanceOnPropertyChanged;
        }
    }
}

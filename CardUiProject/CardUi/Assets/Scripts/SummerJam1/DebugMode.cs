using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Utility;
using JetBrains.Annotations;
using UnityEngine;

public class DebugMode : MonoBehaviourSingleton<DebugMode>, INotifyPropertyChanged
{
    private bool _isDebug;

    public bool IsDebug
    {
        get => _isDebug;
        set
        {
            if (value == _isDebug)
            {
                return;
            }

            _isDebug = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsDebug = !IsDebug;
        }
    }
}

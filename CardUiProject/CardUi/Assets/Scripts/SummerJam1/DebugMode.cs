using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Utility;
using JetBrains.Annotations;
using SummerJam1;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsDebug = !IsDebug;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SummerJam1Context.Instance.Game.CreateDebugMap();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

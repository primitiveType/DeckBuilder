﻿using System.ComponentModel;

namespace Api
{
    public interface IComponent : INotifyPropertyChanged
    {
        IEntity Entity { get; }
    }
}
using System.Collections.Generic;
using System.ComponentModel;

namespace Api
{
    public interface IComponent : INotifyPropertyChanged
    {
        IEntity Entity { get; }
        bool Enabled { get; set; }
    }

    public interface IEventfulComponent : IComponent
    {
        Dictionary<int, int> EventEntrance { get; }
    }
}
